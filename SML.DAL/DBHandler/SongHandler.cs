using SML.DAL.DBEdmx;
using SML.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SML.DAL.DBHandler
{
  public class SongHandler
  {
    /// <summary>
    /// Save Song
    /// </summary>
    /// <param name="song"></param>
    /// <returns></returns>
    public int SaveSong(Song song)
    {
      using (SMLDBEntities context = new SMLDBEntities())
      {
        using (System.Data.Entity.DbContextTransaction transaction = context.Database.BeginTransaction())
        {
          try
          {
            SongTable tblUser = new SongTable()
            {
              SongID = song.SongID,
              SongName = song.SongName,
              Artist = song.Artist,
              Lyrics = song.Lyrics,
              UserId = song.UserId
            };
            context.SongTables.Add(tblUser);
            context.SaveChanges();
            transaction.Commit();
            song.SongID = tblUser.SongID;

            return song.SongID;
          }
          catch (Exception)
          {
            transaction.Rollback();
            return 0;
          }
        }
      }
    }

    public void DeleteSong(object songID)
    {
      throw new NotImplementedException();
    }

    //delete song
    public void DeleteSong(int SongId)
    {
      using (SMLDBEntities dbContext = new SMLDBEntities())
      {
        var singleRec = dbContext.SongTables.FirstOrDefault(x => x.SongID == SongId);// object your want to delete
        dbContext.SongTables.Remove(singleRec);
        dbContext.SaveChanges();
      }
    }

    /// <summary>
    /// Update Song
    /// </summary>
    /// <param name="song"></param>
    /// <returns></returns>
    public int UpdateSong(Song song)
    {
      using (SMLDBEntities context = new SMLDBEntities())
      {
        using (System.Data.Entity.DbContextTransaction transaction = context.Database.BeginTransaction())
        {
          try
          {
            SongTable tblSong = context.SongTables.Where(t => t.SongID == song.SongID).FirstOrDefault();
            if (tblSong != null && tblSong.SongID > 0)
            {
              tblSong.SongID = song.SongID;
              tblSong.SongName = song.SongName ?? tblSong.SongName;
              tblSong.Artist = song.Artist ?? tblSong.Artist;
              tblSong.Lyrics = song.Lyrics ?? tblSong.Lyrics;
              tblSong.UserId = song.UserId > 0 ? song.UserId : tblSong.UserId;
            }
            context.SaveChanges();
            transaction.Commit();
            song.SongID = tblSong.SongID;

            return song.SongID;
          }
          catch (Exception)
          {
            transaction.Rollback();
            return 0;
          }
        }
      }
    }

    /// <summary>
    /// Load Song
    /// </summary>
    /// <param name="SongId"></param>
    /// <returns></returns>
    public Song LoadSong(int SongId)
    {
      Song song = new Song();
      try
      {
        using (SMLDBEntities context = new SMLDBEntities())
        {
          if (SongId > 0)
          {
            SongTable tblSong = context.SongTables.Where(t => t.SongID == SongId).FirstOrDefault();
            if (tblSong != null)
            {
              song.SongID = tblSong.SongID;
              song.SongName = tblSong.SongName;
              song.Artist = tblSong.Artist;
              song.Lyrics = tblSong.Lyrics;
              song.UserId = tblSong.UserId;
            }
          }
        }
      }
      catch (Exception)
      {
        return null;
      }
      return song;
    }

    /// <summary>
    /// Load All Songs
    /// </summary>
    /// <returns></returns>
    public List<Song> LoadAllSongs()
    {
      List<Song> songs = new List<Song>();
      try
      {
        using (SMLDBEntities context = new SMLDBEntities())
        {
          var tblSongs = context.SongTables;
          if (tblSongs != null && tblSongs.Count() > 0)
          {
            foreach (SongTable s in tblSongs)
            {
              Song song = new Song
              {
                SongID = s.SongID,
                SongName = s.SongName,
                Artist = s.Artist,
                Lyrics = s.Lyrics,
                UserId = s.UserId
              };
              songs.Add(song);
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return songs;
    }

    public List<Song> LoadAllSongs(int userId)
    {
      List<Song> songs = null;
      try
      {
        using (SMLDBEntities context = new SMLDBEntities())
        {
          IQueryable<SongTable> tblSongs = context.SongTables.Where(s => s.UserId == userId);
          if (tblSongs != null && tblSongs.Count() > 0)
          {
            foreach (SongTable s in tblSongs)
            {
              Song song = new Song
              {
                SongID = s.SongID,
                SongName = s.SongName,
                Artist = s.Artist,
                Lyrics = s.Lyrics,
                UserId = s.UserId
              };
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return songs;
    }
  }
}