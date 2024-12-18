﻿using RecipeMaster.Model;
using System.Diagnostics;

namespace RecipeMaster;

public partial class RecipeDetailsPage : ContentPage
{
    private Recipe _currentRecipe;

    public RecipeDetailsPage(Recipe recipe)
    {
        InitializeComponent();
        _currentRecipe = recipe;

        RecipeTitle.Text = recipe.Title ?? "No title available";
        RecipeCategory.Text = recipe.Category ?? "No category";
        RecipeArea.Text = recipe.Area ?? "No area";
        RecipeDescription.Text = recipe.Description ?? "No description available";
        RecipeTags.Text = !string.IsNullOrEmpty(recipe.Tags) ? recipe.Tags : "No tags available";
        RecipeSource.Text = !string.IsNullOrEmpty(recipe.Source) ? recipe.Source : "No source available";
        RecipeYoutubeLink.Text = !string.IsNullOrEmpty(recipe.YoutubeLink) ? recipe.YoutubeLink : "No YouTube link available";

        if (!string.IsNullOrEmpty(recipe.MealThumb))
        {
            RecipeImage.Source = recipe.MealThumb;
        }
        else
        {
            RecipeImage.IsVisible = false;
        }

        Debug.WriteLine($"Recipe details:\n" +
                        $"Title: {recipe.Title}\n" +
                        $"Category: {recipe.Category}\n" +
                        $"Area: {recipe.Area}\n" +
                        $"Tags: {recipe.Tags}\n" +
                        $"Source: {recipe.Source}\n" +
                        $"YouTube: {recipe.YoutubeLink}");
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var recipe = new Recipe
        {
            Title = RecipeTitle.Text,
            Description = RecipeDescription.Text,
            Category = RecipeCategory.Text,
            Area = RecipeArea.Text,
            MealThumb = RecipeImage.Source?.ToString(),
            Tags = RecipeTags.Text,
            Source = RecipeSource.Text,
            YoutubeLink = RecipeYoutubeLink.Text
        };

        try
        {
            DatabaseManager.Instance.InsertRecord(recipe);
            await DisplayAlert("Success", "The recipe has been saved successfully!", "OK");
            await Navigation.PopToRootAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error saving recipe: {ex.Message}");
            await DisplayAlert("Error", "Failed to save recipe.", "OK");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        try
        {
            if (Navigation.NavigationStack.Count > 1)
            {
                await Navigation.PopAsync();
            }
            else
            {
                Debug.WriteLine("No pages to pop in navigation stack.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error navigating back: {ex.Message}");
        }
    }

    private async void OnRecipesListClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RecipesListPage());
    }

    private async void OnSourceTapped(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(RecipeSource.Text) && RecipeSource.Text != "No source available")
        {
            try
            {
                Uri uri = new Uri(RecipeSource.Text);
                await Launcher.OpenAsync(uri);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Unable to open source link: " + ex.Message, "OK");
            }
        }
    }

    private async void OnYoutubeTapped(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(RecipeYoutubeLink.Text) && RecipeYoutubeLink.Text != "No YouTube link available")
        {
            try
            {
                Uri uri = new Uri(RecipeYoutubeLink.Text);
                await Launcher.OpenAsync(uri);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Unable to open YouTube link: " + ex.Message, "OK");
            }
        }
    }
}
