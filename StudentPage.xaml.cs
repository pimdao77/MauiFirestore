using MauiFirestore.Services;
using MauiFirestore.ViewModels;

namespace MauiFirestore;

public partial class StudentPage : ContentPage
{
	public StudentPage()
	{
		InitializeComponent();
		var firestoreService = new FirestoreService();
		BindingContext = new StudentViewModel(firestoreService);
	}
}