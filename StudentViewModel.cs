using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MauiFirestore.Models;
using MauiFirestore.Services;
using PropertyChanged;

namespace MauiFirestore.ViewModels;



[AddINotifyPropertyChangedInterface]
public partial class StudentViewModel
{
    FirestoreService _firestoreService;
    public ObservableCollection<SampleModel> Students { get; set; } = [];
    public SampleModel CurrentStudent { get; set; }

    public ICommand Reset { get; set; }
    public ICommand AddOrUpdateCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public StudentViewModel(FirestoreService firestoreService)
    {
        this._firestoreService = firestoreService;
        this.Refresh();
        Reset = new Command(async () =>
        {
            CurrentStudent = new SampleModel();
            await this.Refresh();
        }
        );
        AddOrUpdateCommand = new Command(async () =>
        {
            await this.Save();
            await this.Refresh();
        });
        DeleteCommand = new Command(async () =>
        {
            await this.Delete();
            await this.Refresh();
        });

    }
    public async Task GetAll()
    {
        Students = [];
        var items = await _firestoreService.GetAllStudents();
        foreach (var item in items)
        {
            Students.Add(item);
        }
    }

    public async Task Save()
    {
        if (string.IsNullOrEmpty(CurrentStudent.Id))
        {
            await _firestoreService.InsertSample(this.CurrentStudent);
        }
        else
        {
            await _firestoreService.UpdateSample(this.CurrentStudent);
        }
    }

    private async Task Refresh()
    {
        CurrentStudent = new SampleModel();
        await this.GetAll();
    }

    private async Task Delete()
    {
        await _firestoreService.DeleteSample(this.CurrentStudent.Id);
    }
}
