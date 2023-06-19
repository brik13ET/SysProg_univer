import java.io.IOException;
import java.io.InputStreamReader;
import java.util.Scanner;

public class App {

	public static String e1(String A, int L)
	{
		String result = null;
		for (int i = 0; i < A.length(); i++)
		{
			if (Character.isDigit(A.charAt(i)))
			{
				if (result == null)
					result = "";
				if (L <= 0)
					break;
				result += A.charAt(i);
				L --;
			}
		}
		if (result == null)
			result = "Не найдено";
		return result;
	}

	public static String e2(char C, String[] A)
	{
		String result = null;
		for (String s : A)
			if (s.length() > 0 && s.charAt(s.length() - 1) == C && result == null)
				result = s;
			else if (s.length() > 0 && s.charAt(s.length() - 1) == C && result != null)
			{
				result = "Ошибка";
				break;
			}
		if (result == null)
			result = "";
		return result;
	}
	
	public static String e3(char C, String[] A)
	{
		int cnt = 0;
		for (String s : A) {
			if (
				s.length() > 0 &&
				s.charAt(0) == C &&
				s.charAt(s.length() - 1) == C
			)
				cnt ++;
		}
		return "" + cnt;
	}
	
	public static String e4(int[] A)
	{
		int sum = 0, cnt = 0;
		for (int i : A) {
			if (i < 0)
			{
				cnt ++;
				sum += i;
			}
		}
		return String.format("%d\t%d", sum, cnt);

	}
	public static void main(String[] args) {
		try (
			InputStreamReader rin = new InputStreamReader(System.in);
			Scanner sc = new Scanner(System.in);
		)
		{
			/*
			int L = sc.nextInt();
			sc.nextLine();
			String A = sc.nextLine();
			System.out.println(e1(A, L));
			*/

			/*
			char ch = sc.next(".").charAt(0);
			int cnt = sc.nextInt();
			String[] A = new String[cnt];
			for (int i = 0; i < A.length; i++) {
				A[i] = sc.nextLine();
			}
			System.out.println(e2(ch, A));
			*/

			/*
			char ch = sc.next(".").charAt(0);
			int cnt = sc.nextInt();
			String[] A = new String[cnt];
			for (int i = 0; i < A.length; i++) {
				A[i] = sc.nextLine();
			}
			System.out.println(e3(ch, A));
			*/
			
			/* 
			int cnt = sc.nextInt();
			int[] A = new int[cnt];
			for (int i = 0; i < A.length; i++) {
				A[i] = sc.nextInt();
				sc.nextLine();
			}
			System.out.println(e4(A));
			*/
		}
		catch(IOException ex)
		{
			System.out.println(ex.getMessage());
		}
	}
}
