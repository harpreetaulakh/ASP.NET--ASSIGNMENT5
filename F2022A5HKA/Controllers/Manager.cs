using AutoMapper;
using F2022A5HKA.Data;
using F2022A5HKA.EntityModels;
using F2022A5HKA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using RoleClaim = F2022A5HKA.Data.RoleClaim;

// ************************************************************************************
// WEB524 Project Template V2 == 2227-d88c29db-9845-433b-8cde-f2a3d31afee2
//
// By submitting this assignment you agree to the following statement.
// I declare that this assignment is my own work in accordance with the Seneca Academic
// Policy. No part of this assignment has been copied manually or electronically from
// any other source (including web sites) or distributed to other students.
// ************************************************************************************

namespace F2022A5HKA.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // AutoMapper instance
        public IMapper mapper;

        // Request user property...

        // Backing field for the property
        private RequestUser _user;

        // Getter only, no setter
        public RequestUser User
        {
            get
            {
                // On first use, it will be null, so set its value
                if (_user == null)
                {
                    _user = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);
                }
                return _user;
            }
        }

        // Default constructor...ster
        public Manager()
        {
            // If necessary, add constructor code here

            // Configure the AutoMapper components
            var config = new MapperConfiguration(cfg =>
            {
                // Define the mappings below, for example...
                // cfg.CreateMap<SourceType, DestinationType>();
                // cfg.CreateMap<Employee, EmployeeBase>();

                // Object mapper definitions

                cfg.CreateMap<Genre, GenreBaseViewModel>();

                cfg.CreateMap<Album, AlbumBaseViewModel>();
                cfg.CreateMap<Album, AlbumWithDetailViewModel>();
                cfg.CreateMap<AlbumBaseViewModel, AlbumAddFormViewModel>();
                cfg.CreateMap<AlbumAddViewModel, Album>();

                cfg.CreateMap<Artist, ArtistBaseViewModel>();
                cfg.CreateMap<Artist, ArtistWithDetailViewModel>();
                cfg.CreateMap<ArtistBaseViewModel, ArtistAddFormViewModel>();
                cfg.CreateMap<ArtistAddViewModel, Artist>();

                cfg.CreateMap<Track, TrackBaseViewModel>();
                cfg.CreateMap<Track, TrackWithDetailViewModel>();
                cfg.CreateMap<TrackBaseViewModel, TrackAddFormViewModel>();
                cfg.CreateMap<TrackAddViewModel, Track>();

                cfg.CreateMap<Models.RegisterViewModel, Models.RegisterViewModelForm>();
            });

            mapper = config.CreateMapper();

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }

        // ############################################################
        // RoleClaim

        public List<string> RoleClaimGetAllStrings()
        {
            return ds.RoleClaims.OrderBy(r => r.Name).Select(r => r.Name).ToList();
        }

        // Add methods below
        // Controllers will call these methods
        // Ensure that the methods accept and deliver ONLY view model objects and collections
        // The collection return type is almost always IEnumerable<T>

        // Suggested naming convention: Entity + task/action
        // For example:
        // ProductGetAll()
        // ProductGetById()
        // ProductAdd()
        // ProductEdit()
        // ProductDelete()

        public IEnumerable<GenreBaseViewModel> GenreGetAll()
        {
            var genre = from t in ds.Genres
                        orderby t.Name
                        select t;

            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreBaseViewModel>>(genre);
        }


        public IEnumerable<ArtistBaseViewModel> ArtistGetAll()
        {
            var artist = from t in ds.Artists
                         orderby t.Name
                         select t;

            return mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBaseViewModel>>(artist);
        }

        public ArtistWithDetailViewModel ArtistGetById(int? id)
        {


            //Attempt to fetch the object
            var obj = ds.Artists.Include("Albums").SingleOrDefault(a => a.ArtistId == id);


            if (obj == null)
            {
                return null;
            }
            else
            {
                var result = mapper.Map<Artist, ArtistWithDetailViewModel>(obj);
                result.AlbumNames = obj.Albums.Select(a => a.Name);
                return result;
            }


            // return (obj == null) ? null : mapper.Map<Artist, ArtistWithDetailViewModel>(obj);


        }

        public ArtistWithDetailViewModel ArtistAdd(ArtistAddViewModel newArtist)
        {
            newArtist.Executive = HttpContext.Current.User.Identity.Name;
            var addedItem = ds.Artists.Add(mapper.Map<ArtistAddViewModel, Artist>(newArtist));
            ds.SaveChanges();
            return addedItem == null ? null : mapper.Map<Artist, ArtistWithDetailViewModel>(addedItem);
        }


        public IEnumerable<AlbumBaseViewModel> AlbumGetAll()
        {
            var album = from t in ds.Albums
                        orderby t.Name
                        select t;

            return mapper.Map<IEnumerable<Album>, IEnumerable<AlbumBaseViewModel>>(album);
        }

        public AlbumWithDetailViewModel AlbumGetById(int? id)
        {
            //Attempt to fetch the object
            var obj = ds.Albums.Include("Artists").Include("Tracks").SingleOrDefault(a => a.AlbumId == id);

            if (obj == null)
            {
                return null;
            }
            else
            {

                var result = mapper.Map<Album, AlbumWithDetailViewModel>(obj);
                result.ArtistNames = obj.Artists.Select(a => a.Name);
                return result;

            }
        }

        public AlbumWithDetailViewModel AlbumAdd(AlbumAddViewModel newAlbum)
        {
            var temp = newAlbum.ArtistIds.ToList(); //get the list of artists' id
            temp.Add(newAlbum.ArtistId); //Inside the list, add the new id of newly added album
            newAlbum.ArtistIds = temp; //As you have added a new id, initialize the original file with updated list

            var selectedArtists = new List<Artist>(); //Artist type of selected artist list

            foreach (var selectedArtistId in newAlbum.ArtistIds) //Loop through the newly initialized artistsIds property
            {
                var artist = ds.Artists.Find(selectedArtistId); //find if there is a matching id inside the database
                if (artist != null)
                {
                    selectedArtists.Add(artist); //If it is not null then it adds inside
                }
            }

            if (selectedArtists.Count() > 0)
            {
                if (newAlbum.TrackIds.Count() > 0)
                {
                    var selectedTracks = new List<Track>();

                    foreach (var selectedTrackId in newAlbum.TrackIds)
                    {
                        var track = ds.Tracks.Find(selectedTrackId);
                        if (track != null)
                        {
                            selectedTracks.Add(track);
                        }
                    }

                    newAlbum.Tracks = selectedTracks;
                }

                newAlbum.Artists = selectedArtists;

                newAlbum.Artists = selectedArtists;

                newAlbum.Coordinator = HttpContext.Current.User.Identity.Name;
                var addedAlbum = ds.Albums.Add(mapper.Map<AlbumAddViewModel, Album>(newAlbum));
                ds.SaveChanges();

                return (addedAlbum != null) ? mapper.Map<Album, AlbumWithDetailViewModel>(addedAlbum) : null;
            }

            return null;

        }

        public IEnumerable<TrackBaseViewModel> TrackGetAll()
        {
            var track = from t in ds.Tracks
                        orderby t.Name
                        select t;

            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackBaseViewModel>>(track);
        }

        public IEnumerable<TrackBaseViewModel> TrackGetAllByArtistId(int? id)
        {
            //fetch the artist
            var artist = ds.Artists.Include("Albums.Tracks").SingleOrDefault(a => a.ArtistId == id);

            //Continue?
            if (artist == null)
            {
                return null;
            }

            //Create a collection to hold the results
            var tracks = new List<Track>();

            //Go through
            foreach (var album in artist.Albums)
            {
                tracks.AddRange(album.Tracks);
            }

            tracks = tracks.Distinct().ToList();

            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackBaseViewModel>>(tracks.OrderBy(t => t.Name));
        }

        public TrackWithDetailViewModel TrackGetById(int? id)
        {
            //Attempt to fetch the object
            var obj = ds.Tracks.Include("Albums.Artists").SingleOrDefault(t => t.TrackId == id);

            if (obj == null)
            {
                return null;
            }
            else
            {

                var result = mapper.Map<Track, TrackWithDetailViewModel>(obj);
                result.AlbumNames = obj.Albums.Select(a => a.Name);
                return result;
            }
        }

        public TrackBaseViewModel TrackAdd(TrackAddViewModel newTrack)
        {
            var album = ds.Albums.Find(newTrack.AlbumId);

            if (album != null)
            {
                newTrack.Albums = new List<Album> { album };
            }

            newTrack.Clerk = HttpContext.Current.User.Identity.Name;
            var addedTrack = ds.Tracks.Add(mapper.Map<TrackAddViewModel, Track>(newTrack));

            ds.SaveChanges();
            return (addedTrack != null) ? mapper.Map<Track, TrackWithDetailViewModel>(addedTrack) : null;
        }

        // Add some programmatically-generated objects to the data store
        // Can write one method, or many methods - your decision
        // The important idea is that you check for existing data first
        // Call this method from a controller action/method
        public bool LoadDataArtist()
        {
            var exec = HttpContext.Current.User.Identity.Name;
            var artGenre = ds.Genres.SingleOrDefault(g => g.Name == "Ballad");

            if (ds.Artists.Count() > 0) { return false; }

            // Otherwise...
            // Create and add objects
            ds.Artists.Add(new Artist
            {
                Name = "My Chemical Romance",
                UrlArtist = "https://media.altpress.com/uploads/2018/07/MyChemicalRomance-TheBlackParade.jpg",
                BirthName = "My Chemical Romance",
                BirthOrStartDate = new DateTime(2001, 9, 11),
                Executive = exec,
                Genre = artGenre.Name,
            });

            ds.Artists.Add(new Artist
            {
                Name = "Sung Si-kyung",
                UrlArtist = "https://t1.daumcdn.net/cfile/tistory/999AA0445D2C69342D",
                BirthName = "Sung Si-kyung",
                BirthOrStartDate = new DateTime(1979, 4, 17),
                Executive = exec,
                Genre = artGenre.Name,
            });

            ds.Artists.Add(new Artist
            {
                Name = "Roy Kim",
                UrlArtist = "https://image.mycelebs.com/celeb/sq/284_sq_01.jpg",
                BirthName = "Sangu Kim",
                BirthOrStartDate = new DateTime(1993, 7, 3),
                Executive = exec,
                Genre = artGenre.Name,
            });

            // Save changes
            ds.SaveChanges();

            return true;
        }

        public bool LoadDataAlbum()
        {
            var coord = HttpContext.Current.User.Identity.Name;
            var artGenre = ds.Genres.SingleOrDefault(g => g.Name == "Ballad");

            if (ds.Albums.Count() > 0) { return false; }

            // Otherwise...
            // Create and add objects

            //MCR
            var MCR = ds.Artists.SingleOrDefault(a => a.Name == "My Chemical Romance");
            if (MCR == null) { return false; }
            ds.Albums.Add(new Album
            {
                Name = "The Black Parade",
                Genre = artGenre.Name,
                UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/thumb/e/ea/Blackparadecover.jpg/220px-Blackparadecover.jpg",
                Coordinator = coord,
                ReleaseDate = new DateTime(2006, 10, 24),
                Artists = new List<Artist> { MCR }
            });

            ds.Albums.Add(new Album
            {
                Name = "Three Cheers for Sweet Revenge",
                Genre = artGenre.Name,
                UrlAlbum = "https://images-na.ssl-images-amazon.com/images/I/81aECQlJWwL._AC_SX679_.jpg",
                Coordinator = coord,
                ReleaseDate = new DateTime(2004, 6, 8),
                Artists = new List<Artist> { MCR }
            });

            //Sung Si-kyung
            var SiKyung = ds.Artists.SingleOrDefault(a => a.Name == "Sung Si-kyung");
            if (SiKyung == null) { return false; }
            ds.Albums.Add(new Album
            {
                Name = "best ballads",
                Genre = artGenre.Name,
                UrlAlbum = "https://images-na.ssl-images-amazon.com/images/I/51clHkGLD4L._SX466_.jpg",
                Coordinator = coord,
                ReleaseDate = new DateTime(2015, 11, 25),
                Artists = new List<Artist> { SiKyung }
            });

            ds.Albums.Add(new Album
            {
                Name = "The Ballads",
                Genre = artGenre.Name,
                UrlAlbum = "https://images.genius.com/cf79b51c995cef0e2f0ee8abd40a8a9e.800x749x1.jpg",
                Coordinator = coord,
                ReleaseDate = new DateTime(2006, 10, 10),
                Artists = new List<Artist> { SiKyung }
            });


            //Roy Kim
            var Roy = ds.Artists.SingleOrDefault(a => a.Name == "Roy Kim");
            if (Roy == null) { return false; }
            ds.Albums.Add(new Album
            {
                Name = "The Great Dipper",
                Genre = artGenre.Name,
                UrlAlbum = "https://cdnimg.melon.co.kr/cm/album/images/026/54/724/2654724_500.jpg/melon/resize/282/quality/80/optimize",
                Coordinator = coord,
                ReleaseDate = new DateTime(2015, 12, 4),
                Artists = new List<Artist> { Roy }
            });

            ds.Albums.Add(new Album
            {
                Name = "love love love",
                Genre = artGenre.Name,
                UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/thumb/3/3a/Roy_Kim_-_Love_Love_Love_album_cover.jpg/220px-Roy_Kim_-_Love_Love_Love_album_cover.jpg",
                Coordinator = coord,
                ReleaseDate = new DateTime(2013, 6, 25),
                Artists = new List<Artist> { Roy }
            });

            // Save changes
            ds.SaveChanges();

            return true;
        }

        public bool LoadDataTrack()
        {
            var clerk = HttpContext.Current.User.Identity.Name;
            var artGenre = ds.Genres.SingleOrDefault(g => g.Name == "Ballad");

            if (ds.Tracks.Count() > 0) { return false; }

            // Otherwise...
            // Create and add objects

            //The Black Parade
            var blackParade = ds.Albums.SingleOrDefault(a => a.Name == "The Black Parade");
            if (blackParade == null) { return false; }
            ds.Tracks.Add(new Track
            {
                Name = "Welcome To The Black Parade",
                Clerk = clerk,
                Composers = "Bob Bryar Frank Iero, Ray Toro Gerard Way, Mikey Way, Rob Cavallo, My Chemical Romance",
                Genre = artGenre.Name,
                Albums = new List<Album> { blackParade }
            });

            ds.Tracks.Add(new Track
            {
                Name = "The End.",
                Clerk = clerk,
                Composers = "My Chemical Romance, Bob Bryar, Ray Toro, Mikey Way, Gerard Way, Frank Iero",
                Genre = artGenre.Name,
                Albums = new List<Album> { blackParade }
            });

            ds.Tracks.Add(new Track
            {
                Name = "This Is How I Disappear",
                Clerk = clerk,
                Composers = "My Chemical Romance, Bob Bryar, Ray Toro, Mikey Way, Gerard Way, Frank Iero",
                Genre = artGenre.Name,
                Albums = new List<Album> { blackParade }
            });

            ds.Tracks.Add(new Track
            {
                Name = "Cancer",
                Clerk = clerk,
                Composers = "My Chemical Romance, Bob Bryar, Ray Toro, Mikey Way, Gerard Way, Frank Iero",
                Genre = artGenre.Name,
                Albums = new List<Album> { blackParade }
            });

            ds.Tracks.Add(new Track
            {
                Name = "Mama",
                Clerk = clerk,
                Composers = "My Chemical Romance, Bob Bryar, Ray Toro, Mikey Way, Gerard Way, Frank Iero",
                Genre = artGenre.Name,
                Albums = new List<Album> { blackParade }
            });


            //best ballads
            var fromStars = ds.Albums.SingleOrDefault(a => a.Name == "best ballads");
            if (fromStars == null) { return false; }
            ds.Tracks.Add(new Track
            {
                Name = "Every Moment of You",
                Clerk = clerk,
                Composers = "Sung Si-kyung",
                Genre = artGenre.Name,
                Albums = new List<Album> { fromStars }
            });

            ds.Tracks.Add(new Track
            {
                Name = "The Road To Me",
                Clerk = clerk,
                Composers = "Hyung-suk Kim",
                Genre = artGenre.Name,
                Albums = new List<Album> { fromStars }
            });

            ds.Tracks.Add(new Track
            {
                Name = "You Touched My Heart",
                Clerk = clerk,
                Composers = "Yoon-jong Shin",
                Genre = artGenre.Name,
                Albums = new List<Album> { fromStars }
            });

            ds.Tracks.Add(new Track
            {
                Name = "Two People",
                Clerk = clerk,
                Composers = "Yoon-young Joon",
                Genre = artGenre.Name,
                Albums = new List<Album> { fromStars }
            });

            ds.Tracks.Add(new Track
            {
                Name = "It would be good",
                Clerk = clerk,
                Composers = "Yoon-young Joon",
                Genre = artGenre.Name,
                Albums = new List<Album> { fromStars }
            });

            var greatDipper = ds.Albums.SingleOrDefault(a => a.Name == "The Great Dipper");
            if (greatDipper == null) { return false; }
            ds.Tracks.Add(new Track
            {
                Name = "The Great Dipper",
                Clerk = clerk,
                Composers = "Roy Kim",
                Genre = artGenre.Name,
                Albums = new List<Album> { greatDipper }
            });

            ds.Tracks.Add(new Track
            {
                Name = "Wave",
                Clerk = clerk,
                Composers = "Roy Kim",
                Genre = artGenre.Name,
                Albums = new List<Album> { greatDipper }
            });

            ds.Tracks.Add(new Track
            {
                Name = "Spring Spring Spring",
                Clerk = clerk,
                Composers = "Roy Kim",
                Genre = artGenre.Name,
                Albums = new List<Album> { greatDipper }
            });

            ds.Tracks.Add(new Track
            {
                Name = "Don't Leave Me",
                Clerk = clerk,
                Composers = "Roy Kim",
                Genre = artGenre.Name,
                Albums = new List<Album> { greatDipper }
            });

            ds.Tracks.Add(new Track
            {
                Name = "I Want To Love",
                Clerk = clerk,
                Composers = "Roy Kim",
                Genre = artGenre.Name,
                Albums = new List<Album> { greatDipper }
            });

            // Save changes
            ds.SaveChanges();

            return true;
        }

        public bool LoadDataGenre()
        {
            if (ds.Genres.Count() > 0) { return false; }

            // Otherwise...
            // Create and add objects
            ds.Genres.Add(new Genre { Name = "Rock" });
            ds.Genres.Add(new Genre { Name = "Ballad" });
            ds.Genres.Add(new Genre { Name = "Blues" });
            ds.Genres.Add(new Genre { Name = "Reggae" });
            ds.Genres.Add(new Genre { Name = "Jazz" });
            ds.Genres.Add(new Genre { Name = "R&B" });
            ds.Genres.Add(new Genre { Name = "House" });
            ds.Genres.Add(new Genre { Name = "Techno" });
            ds.Genres.Add(new Genre { Name = "Disco" });
            ds.Genres.Add(new Genre { Name = "Funk" });

            // Save changes
            ds.SaveChanges();

            return true;
        }

        public bool LoadData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            // ############################################################
            // Role claims

            if (ds.RoleClaims.Count() == 0)
            {
                // Add role claims here
                ds.RoleClaims.Add(new RoleClaim { Name = "Executive" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Coordinator" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Clerk" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Staff" });

                ds.SaveChanges();
                done = true;
            }

            return done;
        }

        public bool RemoveData()
        {
            try
            {
                //foreach (var e in ds.RoleClaims)
                //{
                //    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                //}
                foreach (var e in ds.Artists)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveDatabase()
        {
            try
            {
                return ds.Database.Delete();
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    // New "RequestUser" class for the authenticated user
    // Includes many convenient members to make it easier to render user account info
    // Study the properties and methods, and think about how you could use it

    // How to use...

    // In the Manager class, declare a new property named User
    //public RequestUser User { get; private set; }

    // Then in the constructor of the Manager class, initialize its value
    //User = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);

    public class RequestUser
    {
        // Constructor, pass in the security principal
        public RequestUser(ClaimsPrincipal user)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Principal = user;

                // Extract the role claims
                RoleClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                // User name
                Name = user.Identity.Name;

                // Extract the given name(s); if null or empty, then set an initial value
                string gn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                if (string.IsNullOrEmpty(gn)) { gn = "(empty given name)"; }
                GivenName = gn;

                // Extract the surname; if null or empty, then set an initial value
                string sn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                if (string.IsNullOrEmpty(sn)) { sn = "(empty surname)"; }
                Surname = sn;

                IsAuthenticated = true;
                // You can change the string value in your app to match your app domain logic
                IsAdmin = user.HasClaim(ClaimTypes.Role, "Admin") ? true : false;
            }
            else
            {
                RoleClaims = new List<string>();
                Name = "anonymous";
                GivenName = "Unauthenticated";
                Surname = "Anonymous";
                IsAuthenticated = false;
                IsAdmin = false;
            }

            // Compose the nicely-formatted full names
            NamesFirstLast = $"{GivenName} {Surname}";
            NamesLastFirst = $"{Surname}, {GivenName}";
        }

        // Public properties
        public ClaimsPrincipal Principal { get; private set; }
        public IEnumerable<string> RoleClaims { get; private set; }

        public string Name { get; set; }

        public string GivenName { get; private set; }
        public string Surname { get; private set; }

        public string NamesFirstLast { get; private set; }
        public string NamesLastFirst { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public bool IsAdmin { get; private set; }

        public bool HasRoleClaim(string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(ClaimTypes.Role, value) ? true : false;
        }

        public bool HasClaim(string type, string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(type, value) ? true : false;
        }
    }

}