using BELibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BELibrary.Models.View
{
    public class FilmView
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string TrailerLink { get; set; }

        public string Actors { get; set; }

        public string Directors { get; set; }

        public string Country { get; set; }

        public int AgeRestriction { get; set; }

        public DateTime ReleaseDate { get; set; }

        public TimeSpan? Time { get; set; }

        public string BannerUrl { get; set; }

        public string Image { get; set; }

        public string TimeComing { get; set; }

        public string Detail { get; set; }

        public double Price { get; set; }

        public bool IsDelete { get; set; }

        public virtual List<string> FilmMovieTypes { get; set; }

        public virtual List<string> FilmMovieDisplayTypes { get; set; }

        public virtual List<MovieCalendar> MovieCalendars { get; set; }
    }
}