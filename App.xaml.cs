namespace MauiFirestore;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		//MainPage = new AppShell();
		MainPage = new StudentPage();
	}
}
