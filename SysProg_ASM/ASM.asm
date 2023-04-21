public asm_div

_text segment

; int (int ecx, int edx)
asm_div proc
	mov eax, ecx
	mov ecx, edx
	xor edx, edx
	div ecx
	ret
asm_div endp

_text ENDs
end