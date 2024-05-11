using CompactGit.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CompactGit.Components.Pages
{
    public partial class MyRepo : ComponentBase
    {
        public string FindInput { get; set; } = "";
        public GitDb.GitDbContext? Context { get; set; }
        private string selectedVisibility = "All";

        private List<Repository> FilteredRepositories()
        {
            // �������丮 ����� �������� ������ �ۼ��մϴ�.
            // ���� ���, DbContextFactory�� ����Ͽ� DB���� �������丮 ����� ������ �� �ֽ��ϴ�.
            // ������ ����� ����ڰ� ������ Visibility�� ���� ���͸��Ͽ� ��ȯ�մϴ�.
            // ���⼭�� �ӽ÷� �� ����� ��ȯ�մϴ�.
            return new List<Repository>();
        }

        [Parameter]
        public string UserUrl { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IDbContextFactory<GitDb.GitDbContext> DbContextFactory { get; set; }

        [Inject]
        public ICookie Cookie { get; set; } = default!;

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private async Task NewButtonClickAsync()
        {
            NavigationManager.NavigateTo("/create-repo");
        }

        private async Task TypeButtonClickAsync()
        {
            await JSRuntime.InvokeVoidAsync("showVisibilityDropdown");
        }

        private async Task OnVisibilitySelect(string visibility)
        {
            selectedVisibility = visibility;
            // ���õ� Visibility�� ���� �������丮�� �����մϴ�.
            StateHasChanged();
        }

        private List<Repository> GetSortedRepositories()
        {
            var repositories = FilteredRepositories();
            if (selectedVisibility == "Public")
            {
                repositories = repositories.Where(repo => repo.IsPublic).ToList();
            }
            else if (selectedVisibility == "Private")
            {
                repositories = repositories.Where(repo => !repo.IsPublic).ToList();
            }
            return repositories.OrderBy(repo => repo.Name).ToList();
        }

        private async Task SettingsButtonClickAsync(MouseEventArgs e)
        {
            NavigationManager.NavigateTo("/settings/" + UserUrl);
        }

        private async Task ColumnButtonClickAsync(MouseEventArgs e)
        {
            foreach (var repository in Repositories)
            {
                if (repository.Id == selectedRepoId)
                {
                    repository.IsFavorite = !repository.IsFavorite;
                    break;
                }
            }

            // ȭ���� �ٽ� �׸��ϴ�.
            StateHasChanged();
        }

        private async Task RepositoryClickAsync()
        {
            NavigationManager.NavigateTo("/repo-detail");
        }
    }
}