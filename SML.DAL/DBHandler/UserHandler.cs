using SML.DAL.DBEdmx;
using SML.Entities;
using System;
using System.Linq;

namespace SML.DAL.DBHandler
{
  public class UserHandler
  {
    /// <summary>
    /// Save Users
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public int SaveUser(User user)
    {
      using (SMLDBEntities context = new SMLDBEntities())
      {
        using (System.Data.Entity.DbContextTransaction transaction = context.Database.BeginTransaction())
        {
          try
          {
            UserTable tblUser = new UserTable()
            {
              UserID = user.UserID,
              Email = user.Email,
              Password = user.Password,
              UserName = user.UserName
            };
            context.UserTables.Add(tblUser);
            context.SaveChanges();
            transaction.Commit();
            user.UserID = tblUser.UserID;

            return user.UserID;
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
    /// Update Users
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public int UpdateUser(User user)
    {
      using (SMLDBEntities context = new SMLDBEntities())
      {
        using (System.Data.Entity.DbContextTransaction transaction = context.Database.BeginTransaction())
        {
          try
          {
            UserTable tblUser = context.UserTables.Where(t => t.UserID == user.UserID).FirstOrDefault();
            if (tblUser != null && tblUser.UserID > 0)
            {
              tblUser.UserID = user.UserID;
              tblUser.Email = user.Email ?? tblUser.Email;
              tblUser.Password = user.Password ?? tblUser.Password;
              tblUser.UserName = user.UserName ?? tblUser.UserName;
            }
            context.SaveChanges();
            transaction.Commit();
            user.UserID = tblUser.UserID;
            return user.UserID;
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
    /// Load User
    /// </summary>
    /// <param name="UserId"></param>
    /// <returns></returns>
    public User LoadUser(int UserId)
    {
      User user = new User();
      try
      {
        using (SMLDBEntities context = new SMLDBEntities())
        {
          if (UserId > 0)
          {
            UserTable tblUser = context.UserTables.Where(t => t.UserID == UserId).FirstOrDefault();
            if (tblUser != null)
            {
              user.UserID = tblUser.UserID;
              user.UserName = tblUser.UserName;
              user.Password = tblUser.Password;
              user.Email = tblUser.Email;
            }
          }
        }
      }
      catch (Exception)
      {
        return null;
      }
      return user;
    }

    public User UserLogin(User user)
    {
      User userDetail = new User();
      try
      {
        using (SMLDBEntities context = new SMLDBEntities())
        {
          UserTable tblUser = context.UserTables.Where(u => (u.Email == user.Email || u.UserName == user.UserName) && u.Password == user.Password).FirstOrDefault();
          if (tblUser != null)
          {
            userDetail.UserID = tblUser.UserID;
            userDetail.UserName = tblUser.UserName;
            userDetail.Password = tblUser.Password;
            userDetail.Email = tblUser.Email;
          }
        }
      }
      catch (Exception)
      {
        return null;
      }
      return userDetail;
    }
  }
}