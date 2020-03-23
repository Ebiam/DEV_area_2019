using Microsoft.EntityFrameworkCore;
using Area_api.Models;
using Area_api.Models.Links;
using Area_api.Models.Apis;

namespace Area_api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>().HasData(new Service { id = 5, name="Imgur", link= "https://api.imgur.com/oauth2/authorize?response_type=token&client_id=41ebd49599744a4&client_secret=144c3ef1384ad0a5c239dd126f4ac06570bb8946&callback_url=http://localhost:8081/token&auth_url=https://api.imgur.com/oauth2/authorize&access_token_url=https://api.imgur.com/oauth2/token", clientId = "41ebd49599744a4", clientSecret = "144c3ef1384ad0a5c239dd126f4ac06570bb8946" });
            modelBuilder.Entity<Service>().HasData(new Service { id = 3, name = "Trello", link = "https://trello.com/1/authorize?expiration=never&name=Area&scope=read,write,account&response_type=token&key=7b20ee1cf6225cb2dab5bcd4025c7d83&return_url=http://localhost:8081/token", clientId = "7b20ee1cf6225cb2dab5bcd4025c7d83" });
            modelBuilder.Entity<Service>().HasData(new Service { id = 2, name = "Spotify", link = "https://accounts.spotify.com/authorize?response_type=code&client_id=efc261a3bef74cd395f334fe0639a723&scope=user-read-playback-state%20ugc-image-upload%20user-read-playback-state%20user-modify-playback-state%20user-read-currently-playing%20streaming%20app-remote-control%20user-read-email%20user-read-private%20playlist-read-collaborative%20playlist-modify-public%20playlist-read-private%20playlist-modify-private%20user-library-modify%20user-library-read%20user-top-read%20user-read-recently-played%20user-follow-read%20user-follow-modify&redirect_uri=http://localhost:8081/token2", link2 = "https://accounts.spotify.com/api/token?grant_type=authorization_code&redirect_uri=http://localhost:8081/token2&code=", clientId = "efc261a3bef74cd395f334fe0639a723", clientSecret = "f5988c3dab204d9a85990af17ee95b7b" });
            modelBuilder.Entity<Service>().HasData(new Service { id = 4, name = "Github", link = "https://github.com/login/oauth/authorize?client_id=c961210e5b7ec1a6ef85&redirect_uri=&http://localhost:8081/token2&scope=repo%20repo_deployment%20user%20public_repo", link2= "https://github.com/login/oauth/access_token?client_id=c961210e5b7ec1a6ef85&client_secret=567f6b6891645292f907cc70a1d21628c0611659&redirect_uri=http://localhost:8081/token2&scope=repo%20repo_deployment%20user%20public_repo&code=", clientId= "c961210e5b7ec1a6ef85", clientSecret = "567f6b6891645292f907cc70a1d21628c0611659" });
            modelBuilder.Entity<Service>().HasData(new Service { id = 1, name = "Yammer", link = "https://www.yammer.com/oauth2/authorize?client_id=SGr2e0d8rwlN1g5ZZmQaQ&response_type=code&redirect_uri=http://localhost:8081/token2", link2= "https://www.yammer.com/oauth2/access_token?client_id=SGr2e0d8rwlN1g5ZZmQaQ&client_secret=UotSqwzsT05jNN89XiAwIpiGEB7r1CjcgHngIG01TE&grant_type=authorization_code&code=", clientId = "SGr2e0d8rwlN1g5ZZmQaQ", clientSecret = "UotSqwzsT05jNN89XiAwIpiGEB7r1CjcgHngIG01TE" });

            modelBuilder.Entity<User>().HasData(new User { id = 42, username = "Enzo", password = "pass" });

            modelBuilder.Entity<UserServices>().HasData(new UserServices { id = 1, serviceId = 5, userId = 42});

            modelBuilder.Entity<Action>().HasData(new Action { id = 1, name = "User post new image", description = "Check if a new image have been posted by the user", serviceId = 5 });
            modelBuilder.Entity<Action>().HasData(new Action { id = 8, name = "User removes an image", description = "Check if user removed an image", serviceId = 5 });
            modelBuilder.Entity<Action>().HasData(new Action { id = 2, name = "User reputation", description = "Check if the user won a reputation point", serviceId = 5 });
            modelBuilder.Entity<Action>().HasData(new Action { id = 3, name = "Reply notification", description = "Check if the user have a new reply notification", serviceId = 5 });

            modelBuilder.Entity<Action>().HasData(new Action { id = 4, name = "New Board", description = "Check if the user have a new board", serviceId = 3 }); // https://api.trello.com/1/members/me/?key=7b20ee1cf6225cb2dab5bcd4025c7d83&token=759ea01b66e2c72a2dec94b72910cb2beb3e8237ae56bc2bf43952994e439a65    idBoards
            modelBuilder.Entity<Action>().HasData(new Action { id = 9, name = "Delete Board", description = "Check if the user delete a board", serviceId = 3 }); // https://api.trello.com/1/members/me/?key=7b20ee1cf6225cb2dab5bcd4025c7d83&token=759ea01b66e2c72a2dec94b72910cb2beb3e8237ae56bc2bf43952994e439a65    idBoards

            modelBuilder.Entity<Action>().HasData(new Action { id = 5, name = "User new repo", description = "Check if the user have a new repo", serviceId = 4 }); // https://api.github.com/user/repos

            modelBuilder.Entity<Action>().HasData(new Action { id = 6, name = "User notification", description = "Check if the user have a new notification", serviceId = 1 }); // https://api.github.com/user/repos
            modelBuilder.Entity<Action>().HasData(new Action { id = 7, name = "User new playlist", description = "Check if the user have added a new playlist", serviceId = 2 }); // https://api.github.com/user/repos
            modelBuilder.Entity<Action>().HasData(new Action { id = 10, name = "User deleted a playlist", description = "Check if the user have deleted a playlist", serviceId = 2 });

            modelBuilder.Entity<Reaction>().HasData(new Reaction { id = 1, name = "Create board", description = "Create a new board PARAM={name} name: name of the board (a random number will be added)", serviceId = 3 }); // https://api.trello.com/1/boards/?name=lol&defaultLabels=true&defaultLists=false&keepFromSource=none&prefs_permissionLevel=private&prefs_voting=disabled&prefs_comments=members&prefs_invitations=members&prefs_selfJoin=true&prefs_cardCovers=true&prefs_background=blue&prefs_cardAging=regular&key=7b20ee1cf6225cb2dab5bcd4025c7d83&token=759ea01b66e2c72a2dec94b72910cb2beb3e8237ae56bc2bf43952994e439a65
            modelBuilder.Entity<Reaction>().HasData(new Reaction { id = 2, name = "Create repository", description = "Create a new repository PARAM={name} name: name of the repo (a random number will be added)", serviceId = 4 }); // https://api.trello.com/1/boards/?name=lol&defaultLabels=true&defaultLists=false&keepFromSource=none&prefs_permissionLevel=private&prefs_voting=disabled&prefs_comments=members&prefs_invitations=members&prefs_selfJoin=true&prefs_cardCovers=true&prefs_background=blue&prefs_cardAging=regular&key=7b20ee1cf6225cb2dab5bcd4025c7d83&token=759ea01b66e2c72a2dec94b72910cb2beb3e8237ae56bc2bf43952994e439a65


        }

        public DbSet<User> Users { get; set; }

        public DbSet<Service> Services { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Trigger> Triggers { get; set; }


        public DbSet<UserServices> UserServices { get; set; }

        public DbSet<ApiToken> Apis { get; set; }
    }
}
