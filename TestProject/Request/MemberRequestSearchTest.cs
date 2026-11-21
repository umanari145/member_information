using System.Xml.Linq;
using WinFormsApp2.Request;

namespace TestProject.Request
{
    [TestClass]
    public class MemberRequestSearchTest
    {

        public static object[][] GetPatterns()
        {
            return new[]
            {
                new object?[] { "John", 20, 30, "男性", " WHERE Name LIKE @Name AND Age >= @AgeFrom AND Age <= @AgeTo AND Gender = @Gender" },
                new object?[] { null, null, null, null, "" },
                new object?[] { "山田", 20, null, null, " WHERE Name LIKE @Name AND Age >= @AgeFrom" },
                new object?[] { null, null, 40, "女性", " WHERE Age <= @AgeTo AND Gender = @Gender" },
            }!;
        }

        [TestMethod]
        [DynamicData(nameof(GetPatterns))]
        public void Build_条件句のWHERE句の判定(
            string? name,
            int? ageFrom,
            int? ageTo,
            string? gender,
            string expectedWhereClause)
        {
            var memberRequest = new MemberRequestSearch
            {
                Name = name,
                AgeFrom = ageFrom,
                AgeTo = ageTo,
                Gender = gender
            };

            memberRequest.Build();
            string whereClause = memberRequest.ToWhereClause();
            // Console.WriteLine($"実際のWHERE句: {whereClause}");
            // Console.WriteLine($"期待値のWHERE句: {expectedWhereClause}");
            LikeValCheck("Name", name, memberRequest);
            ValCheck("AgeFrom", ageFrom, memberRequest);
            ValCheck("AgeTo", ageTo, memberRequest);
            ValCheck("Gender", gender, memberRequest);
            Assert.AreEqual(expectedWhereClause, whereClause);
        }

        private static void LikeValCheck(string key, object? expectedVal, MemberRequestSearch memberRequest)
        {
            Assert.AreEqual(
                // 期待値
                memberRequest.Parameters.ContainsKey($"@{key}") ? $"%{expectedVal}%" : null,
                // 実際の値
                memberRequest.Parameters.TryGetValue($"@{key}", out object? v) ? v : null
            // 下記の値と同じ意味だが、TryGetValueの方が安全
            // memberRequest.Parameters.ContainsKey($"@{key}") ? memberRequest.Parameters[$"@{key}"] : null
            );
        }

        private static void ValCheck(string key, object? expcetedVal, MemberRequestSearch memberRequest)
        {
            Assert.AreEqual(
                // 期待値
                memberRequest.Parameters.ContainsKey($"@{key}") ? expcetedVal : null,
                // 実際の値
                memberRequest.Parameters.TryGetValue($"@{key}", out var v) ? v : null
            // 下記の値と同じ意味だが、TryGetValueの方が安全
            // memberRequest.Parameters.ContainsKey($"@{key}") ? memberRequest.Parameters[$"@{key}"] : null
            );
        }
    }
}
