using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using MediaManager.Library;
using Realms;

namespace Awesome.Player.Models
{
	//public class MediaModel
	//{
	//	public string Title { get; set; }
	//	public string Caption { get; set; }
	//	public string Duration { get; set; }
	//	public string Views { get; set; }
	//	public string Image { get; set; }
	//	public string Link { get; set; }
	//	public List<Note> Notes { get; set; }
	//	//public TimeSpan Position { get; set; }
	//	public string Extension { get; set; }
	//}

	public class MediaModel : RealmObject
	{
		public string Id { get; set; }

		public string Album { get; set; }
		public string AlbumArtist { get; set; }
		//public object AlbumImage { get; set; }
		public string AlbumImageUri { get; set; }
		public string Artist { get; set; }
		//public object Image { get; set; }
		public string ImageUri { get; set; }
		public string Author { get; set; }
		public string Compilation { get; set; }
		public string Composer { get; set; }
		//public DateTimeOffset Date { get; set; }
		public int DiscNumber { get; set; }
		//public object DisplayImage { get; set; }
		public string DisplayImageUri { get; set; }
		public string DisplayDescription { get; set; }
		public string DisplaySubtitle { get; set; }
		public string DisplayTitle { get; set; }
		//public DownloadStatus DownloadStatus { get; set; }
		public long Duration { get; set; }
		//public object Extras { get; set; }
		public string Genre { get; set; }

		[PrimaryKey]
		public string MediaUri { get; set; }

		public int NumTracks { get; set; }
		//public object Rating { get; set; }
		public string Title { get; set; }
		public int TrackNumber { get; set; }
		//public object UserRating { get; set; }
		public string Writer { get; set; }
		public int Year { get; set; }
		public string FileExtension { get; set; }
		public string FileName { get; set; }
		//public MediaType MediaType { get; set; }
		//public MediaLocation MediaLocation { get; set; }

		public static explicit operator MediaModel(MediaItem mediaItem)
		{
			return new MediaModel()
			{
				Id = mediaItem.Id,
				Album = mediaItem.Album,
				AlbumArtist = mediaItem.AlbumArtist,
				//AlbumImage = mediaItem.AlbumImage,
				AlbumImageUri = mediaItem.AlbumImageUri,
				Artist = mediaItem.Artist,
				Author = mediaItem.Author,
				Duration = mediaItem.Duration.Ticks,
				FileExtension = mediaItem.FileExtension,
				Title = mediaItem.Title,
				Compilation = mediaItem.Compilation,
				Composer = mediaItem.Composer,
				//Date = mediaItem.Date,
				DiscNumber = mediaItem.DiscNumber,
				DisplayDescription = mediaItem.DisplayDescription,
				//DisplayImage = mediaItem.DisplayImage,
				DisplayImageUri = mediaItem.DisplayImageUri,
				DisplaySubtitle = mediaItem.DisplaySubtitle,
				DisplayTitle = mediaItem.DisplayTitle,
				//DownloadStatus = mediaItem.DownloadStatus,
				//Extras = mediaItem.Extras,
				FileName = mediaItem.FileName,
				Genre = mediaItem.Genre,
				//Image = mediaItem.Image,
				ImageUri = mediaItem.ImageUri,
				//MediaLocation = mediaItem.MediaLocation,
				//MediaType = mediaItem.MediaType,
				MediaUri = mediaItem.MediaUri,
				NumTracks = mediaItem.NumTracks,
				//Rating = mediaItem.Rating,
				TrackNumber = mediaItem.TrackNumber,
				//UserRating = mediaItem.UserRating,
				Writer = mediaItem.Writer,
				Year = mediaItem.Year
			};
		}
	}
}
