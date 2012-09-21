﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Common.Extensions;

namespace ServiceStack.Examples.ServiceModel.Types
{
	[DataContract(Namespace = ExampleConfig.DefaultNamespace)]
	public class Movie 
	{
		public Movie()
		{
			this.Genres = new List<string>();
		}
		
		public string Id { get; set; }		
		public string Title { get; set; }		
		public decimal Rating { get; set; }		
		public string Director { get; set; }		
		public DateTime ReleaseDate { get; set; }		
		public string TagLine { get; set; }		
		public List<string> Genres { get; set; }

		/// <summary>
		/// AutoGen ReSharper code, only required by tests
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(Movie other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other.Id, Id) && Equals(other.Title, Title) && other.Rating == Rating && Equals(other.Director, Director) && other.ReleaseDate.Equals(ReleaseDate) && Equals(other.TagLine, TagLine) && Genres.EquivalentTo(other.Genres);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (Movie)) return false;
			return Equals((Movie) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = (Id != null ? Id.GetHashCode() : 0);
				result = (result*397) ^ (Title != null ? Title.GetHashCode() : 0);
				result = (result*397) ^ Rating.GetHashCode();
				result = (result*397) ^ (Director != null ? Director.GetHashCode() : 0);
				result = (result*397) ^ ReleaseDate.GetHashCode();
				result = (result*397) ^ (TagLine != null ? TagLine.GetHashCode() : 0);
				result = (result*397) ^ (Genres != null ? Genres.GetHashCode() : 0);
				return result;
			}
		}
	}
}