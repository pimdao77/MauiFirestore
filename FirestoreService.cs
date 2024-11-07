using System;
using Google.Cloud.Firestore;
using MauiFirestore.Models;

namespace MauiFirestore.Services;

public class FirestoreService
{
 private FirestoreDb db;
    public string StatusMessage;

    public FirestoreService()
    {
        this.SetupFireStore();
    }
private async Task SetupFireStore()
    {
        if (db == null)
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("dx212-2024-4051b-firebase-adminsdk-ken2w-e23e982780.json");
            var reader = new StreamReader(stream);
            var contents = reader.ReadToEnd();
            db = new FirestoreDbBuilder
            {
                ProjectId = "dx212-2024-4051b",

                JsonCredentials = contents
            }.Build();
        }
    }
public async Task<List<SampleModel>?> GetAllStudents()
    {
        try
        {
            await SetupFireStore();
            var data = await db.Collection("Students").GetSnapshotAsync();
            var Students = data.Documents.Select(doc =>
            {
                var sample = new SampleModel();
                sample.Id = doc.Id;
                sample.Code = doc.GetValue<string>("Code");
                sample.Name = doc.GetValue<string>("Name");
                return sample;
            }).ToList();
            return Students;
        }
        catch (Exception ex)
        {

            StatusMessage = $"Error: {ex.Message}";
        }
        return null;
    }

    public async Task InsertSample(SampleModel Students)
    {
        try
        {
            await SetupFireStore();
            var sampleData = new Dictionary<string, object>
            {
                { "Code", Students.Code },
                { "Name", Students.Name }
                // Add more fields as needed
            };

            await db.Collection("Students").AddAsync(sampleData);
        }
        catch (Exception ex)
        {

            StatusMessage = $"Error: {ex.Message}";
        }
    }

    public async Task UpdateSample(SampleModel Students)
    {
        try
        {
            await SetupFireStore();

            // Manually create a dictionary for the updated data
            var sampleData = new Dictionary<string, object>
            {
                { "Code", Students.Code },
                { "Name", Students.Name }
                // Add more fields as needed
            };

            // Reference the document by its Id and update it
            var docRef = db.Collection("Students").Document(Students.Id);
            await docRef.SetAsync(sampleData, SetOptions.Overwrite);

            StatusMessage = "Students successfully updated!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }

    public async Task DeleteSample(string id)
    {
        try
        {
            await SetupFireStore();

            // Reference the document by its Id and delete it
            var docRef = db.Collection("Students").Document(id);
            await docRef.DeleteAsync();

            StatusMessage = "Students successfully deleted!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }


}
