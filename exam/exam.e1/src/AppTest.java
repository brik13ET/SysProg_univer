import static org.junit.Assert.assertEquals;

import org.junit.Test;

public class AppTest {
	@Test
	public void testE1() {
		assertEquals("1231"         ,App.e1("abcdef123123123",	4));
		assertEquals("12"           ,App.e1("abcdef123123123",	2));
		assertEquals(""             ,App.e1("abcdef123123123",	0));
		assertEquals(""             ,App.e1("abcdef123123123",	-1));
		assertEquals("12"           ,App.e1("abcdef12",			4));
		assertEquals("Не найдено"   ,App.e1("abcdef",			1));
		assertEquals("Не найдено"   ,App.e1("abcdef",			0));
		assertEquals("Не найдено"   ,App.e1("abcdef",			-1));
	}

    @Test
    public void testE2() {
        
		assertEquals( "aaab"	,App.e2('b', new String[] {"aaab",	"aaaa",	""}));
		assertEquals( "Ошибка"	,App.e2('b', new String[] {"aaab",	"aaaab",""}));
		assertEquals( ""		,App.e2('b', new String[] {"aaa",		"aaaa",	""}));
		assertEquals( ""		,App.e2('b', new String[] {"",		"",		""}));
    }

	@Test
	public void testE3() {
		
		assertEquals( "0", App.e3('b', new String[] {"aaab",	"aaaa",	""}));
		assertEquals( "1", App.e3('b', new String[] {"baaab",	"aaaab",""}));
		assertEquals( "2", App.e3('a', new String[] {"aaa",		"aaaa",	""}));
		assertEquals( "0", App.e3('a', new String[] {"",		"",		""}));
	}

	@Test
	public void testE4() {
		assertEquals( "-3\t3", App.e4(new int[] {0, -1, -1, -1, 123, 123, 123}));
		assertEquals( "-3\t3", App.e4(new int[] {-1, -1, -1}));
		assertEquals( "0\t0", App.e4(new int[] {0, 0}));
		assertEquals( "0\t0", App.e4(new int[] { }));
	}

	@Test
	public void testE5() {
		assertEquals( "1", App.e5(new int[] {0, -1, -1, -1, 123, 123, 1}));
		assertEquals( "0", App.e5(new int[] {-1, -1, -1}));
		assertEquals( "0", App.e5(new int[] {0, 0}));
		assertEquals( "0", App.e5(new int[] { }));
	}

	@Test
	public void testE6() {
		assertEquals( "-1000 -22 -2", App.e6(new int[] {0, -1, -2, -22, 123, 124, -1000}));
		assertEquals( "-2", App.e6(new int[] {-1, -2, -1}));
		assertEquals( "", App.e6(new int[] {0, 0}));
		assertEquals( "", App.e6(new int[] { }));
	}
}
