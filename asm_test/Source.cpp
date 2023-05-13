#include <stdio.h>
#include <Windows.h>

int main(int argc, char* argv[])
{
	HMODULE lib = LoadLibraryA("ASM_dll");
	if (lib == NULL)
	{
		fprintf_s(stderr, "failed loadlib\n");
		return -1;
	}
	int (*func)(int, int) = (int(*)(int, int))GetProcAddress(lib, "call_asm_div");
	if (func == NULL)
	{
		fprintf_s(stderr, "failed finfunc\n");
		return -2;
	}
	printf("f(): %d\n", func(123, 32));
	return 0;;
}