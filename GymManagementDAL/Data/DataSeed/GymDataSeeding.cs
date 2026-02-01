using System.Text.Json;
using System.Text.Json.Serialization;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDataSeeding
    {
        public static bool SeedData(GymDbContext gymDbContext)
        {
			try
			{
				if (!gymDbContext.Categories.Any())
				{
					var categories = LoadDataFromJsonFile<Category>("categories.json");
                    gymDbContext.Categories.AddRange(categories);
				}
				if (!gymDbContext.Plans.Any())
				{
                    var plans = LoadDataFromJsonFile<Plan>("plans.json");
                    gymDbContext.Plans.AddRange(plans);
                }
				return gymDbContext.SaveChanges() > 0;
			}
			catch (Exception)
			{
				return false;
			}
        }
		
		private static IEnumerable<T> LoadDataFromJsonFile<T>(string fileName)
		{
			string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\", fileName);

			if(!File.Exists(filePath)) throw new FileNotFoundException(fileName);

			var jsonOptions = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};

			jsonOptions.Converters.Add(new JsonStringEnumConverter());

			return JsonSerializer.Deserialize<IEnumerable<T>>(File.ReadAllText(filePath), jsonOptions) ?? [];
        }
    }
}
