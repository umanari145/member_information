using Microsoft.EntityFrameworkCore;
using WinFormsApp2.Data;

namespace TestProject;

[TestClass]
public class MemberTests
{
    // テスト専用DB。本番の "MembersDb" とは別名
    private const string _testDbName = "MembersDb_Test";

    private const string _testConnectionString =
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=" + _testDbName +
        ";Integrated Security=True;TrustServerCertificate=True;Connect Timeout=60;";

    private AppDbContext _context = default!;

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(_testConnectionString)
            .Options;
        return new AppDbContext(options);
    }

    // クラス内の全テストの前に1回だけ:テーブルをマイグレーションで作成
    [ClassInitialize]
    public static void ClassSetup(TestContext _)
    {
        // 本番に誤接続しないためのガード(DB名が完全一致のときだけ実行)
        using var context = CreateContext();
        var dbName = context.Database.GetDbConnection().Database;
        if (dbName != _testDbName)
            throw new InvalidOperationException($"テスト用DB以外には接続できません: {dbName}");

        context.Database.Migrate();
    }

    // 各テストの前に:データを空にする
    [TestInitialize]
    public void Setup()
    {
        _context = CreateContext();
        _context.Members.ExecuteDelete();
    }

    [TestCleanup]
    public void Cleanup() => _context.Dispose();

    [TestMethod]
    public void 会員を保存して取得できる()
    {
        _context.Members.Add(new Member { Name = "Taro", Age = 30, Gender = "男" });
        _context.SaveChanges();

        // 別のコンテキストで読み直し、DBに実際に保存されたことを確認
        using var readContext = CreateContext();
        var member = readContext.Members.Single();

        Assert.AreEqual("Taro", member.Name);
        Assert.AreEqual(30, member.Age);
        Assert.AreEqual("男", member.Gender);
        Assert.IsTrue(member.MemberId > 0);   // 自動採番されている
    }
}