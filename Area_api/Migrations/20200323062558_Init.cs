using Microsoft.EntityFrameworkCore.Migrations;

namespace Area_api.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    serviceId = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Apis",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    userId = table.Column<int>(nullable: false),
                    serviceId = table.Column<int>(nullable: false),
                    accessToken = table.Column<string>(nullable: true),
                    refreshToken = table.Column<string>(nullable: true),
                    username = table.Column<string>(nullable: true),
                    accountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apis", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    userId = table.Column<int>(nullable: false),
                    actionId = table.Column<int>(nullable: false),
                    actionParam = table.Column<string>(nullable: true),
                    reactionId = table.Column<int>(nullable: false),
                    reactionParam = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    serviceId = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: true),
                    link = table.Column<string>(nullable: true),
                    link2 = table.Column<string>(nullable: true),
                    clientId = table.Column<string>(nullable: true),
                    clientSecret = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Triggers",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    init = table.Column<bool>(nullable: false),
                    areaId = table.Column<int>(nullable: false),
                    IsAction = table.Column<bool>(nullable: false),
                    act_reactId = table.Column<int>(nullable: false),
                    Ivalue = table.Column<int>(nullable: false),
                    Svalue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Triggers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserServices",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    userId = table.Column<int>(nullable: false),
                    serviceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserServices", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "id", "description", "name", "serviceId" },
                values: new object[] { 1, "Check if a new image have been posted by the user", "User post new image", 5 });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "id", "description", "name", "serviceId" },
                values: new object[] { 7, "Check if the user have added a new playlist", "User new playlist", 2 });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "id", "description", "name", "serviceId" },
                values: new object[] { 6, "Check if the user have a new notification", "User notification", 1 });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "id", "description", "name", "serviceId" },
                values: new object[] { 5, "Check if the user have a new repo", "User new repo", 4 });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "id", "description", "name", "serviceId" },
                values: new object[] { 9, "Check if the user delete a board", "Delete Board", 3 });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "id", "description", "name", "serviceId" },
                values: new object[] { 10, "Check if the user have deleted a playlist", "User deleted a playlist", 2 });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "id", "description", "name", "serviceId" },
                values: new object[] { 3, "Check if the user have a new reply notification", "Reply notification", 5 });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "id", "description", "name", "serviceId" },
                values: new object[] { 2, "Check if the user won a reputation point", "User reputation", 5 });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "id", "description", "name", "serviceId" },
                values: new object[] { 8, "Check if user removed an image", "User removes an image", 5 });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "id", "description", "name", "serviceId" },
                values: new object[] { 4, "Check if the user have a new board", "New Board", 3 });

            migrationBuilder.InsertData(
                table: "Reactions",
                columns: new[] { "id", "description", "name", "serviceId" },
                values: new object[] { 1, "Create a new board PARAM={name} name: name of the board (a random number will be added)", "Create board", 3 });

            migrationBuilder.InsertData(
                table: "Reactions",
                columns: new[] { "id", "description", "name", "serviceId" },
                values: new object[] { 2, "Create a new repository PARAM={name} name: name of the repo (a random number will be added)", "Create repository", 4 });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "id", "clientId", "clientSecret", "link", "link2", "name" },
                values: new object[] { 1, "SGr2e0d8rwlN1g5ZZmQaQ", "UotSqwzsT05jNN89XiAwIpiGEB7r1CjcgHngIG01TE", "https://www.yammer.com/oauth2/authorize?client_id=SGr2e0d8rwlN1g5ZZmQaQ&response_type=code&redirect_uri=http://localhost:8081/token2", "https://www.yammer.com/oauth2/access_token?client_id=SGr2e0d8rwlN1g5ZZmQaQ&client_secret=UotSqwzsT05jNN89XiAwIpiGEB7r1CjcgHngIG01TE&grant_type=authorization_code&code=", "Yammer" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "id", "clientId", "clientSecret", "link", "link2", "name" },
                values: new object[] { 5, "41ebd49599744a4", "144c3ef1384ad0a5c239dd126f4ac06570bb8946", "https://api.imgur.com/oauth2/authorize?response_type=token&client_id=41ebd49599744a4&client_secret=144c3ef1384ad0a5c239dd126f4ac06570bb8946&callback_url=http://localhost:8081/token&auth_url=https://api.imgur.com/oauth2/authorize&access_token_url=https://api.imgur.com/oauth2/token", null, "Imgur" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "id", "clientId", "clientSecret", "link", "link2", "name" },
                values: new object[] { 3, "7b20ee1cf6225cb2dab5bcd4025c7d83", null, "https://trello.com/1/authorize?expiration=never&name=Area&scope=read,write,account&response_type=token&key=7b20ee1cf6225cb2dab5bcd4025c7d83&return_url=http://localhost:8081/token", null, "Trello" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "id", "clientId", "clientSecret", "link", "link2", "name" },
                values: new object[] { 2, "efc261a3bef74cd395f334fe0639a723", "f5988c3dab204d9a85990af17ee95b7b", "https://accounts.spotify.com/authorize?response_type=code&client_id=efc261a3bef74cd395f334fe0639a723&scope=user-read-playback-state%20ugc-image-upload%20user-read-playback-state%20user-modify-playback-state%20user-read-currently-playing%20streaming%20app-remote-control%20user-read-email%20user-read-private%20playlist-read-collaborative%20playlist-modify-public%20playlist-read-private%20playlist-modify-private%20user-library-modify%20user-library-read%20user-top-read%20user-read-recently-played%20user-follow-read%20user-follow-modify&redirect_uri=http://localhost:8081/token2", "https://accounts.spotify.com/api/token?grant_type=authorization_code&redirect_uri=http://localhost:8081/token2&code=", "Spotify" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "id", "clientId", "clientSecret", "link", "link2", "name" },
                values: new object[] { 4, "c961210e5b7ec1a6ef85", "567f6b6891645292f907cc70a1d21628c0611659", "https://github.com/login/oauth/authorize?client_id=c961210e5b7ec1a6ef85&redirect_uri=&http://localhost:8081/token2&scope=repo%20repo_deployment%20user%20public_repo", "https://github.com/login/oauth/access_token?client_id=c961210e5b7ec1a6ef85&client_secret=567f6b6891645292f907cc70a1d21628c0611659&redirect_uri=http://localhost:8081/token2&scope=repo%20repo_deployment%20user%20public_repo&code=", "Github" });

            migrationBuilder.InsertData(
                table: "UserServices",
                columns: new[] { "id", "serviceId", "userId" },
                values: new object[] { 1, 5, 42 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "password", "username" },
                values: new object[] { 42, "pass", "Enzo" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "Apis");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Triggers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserServices");
        }
    }
}
