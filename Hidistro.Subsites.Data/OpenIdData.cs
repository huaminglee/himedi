﻿using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.Subsites.Data
{
    public class OpenIdData : OpenIdProvider
    {
        Database database = DatabaseFactory.CreateDatabase();

        public override void DeleteSettings(string openIdType)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_OpenIdSettings WHERE LOWER(OpenIdType)=LOWER(@OpenIdType) AND UserId=@UserId");
            database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType.ToLower());
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public override IList<string> GetConfigedTypes()
        {
            IList<string> list = new List<string>();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT OpenIdType FROM distro_OpenIdSettings WHERE UserId=@UserId");
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(reader.GetString(0).ToLower());
                }
            }
            return list;
        }

        public override OpenIdSettingsInfo GetOpenIdSettings(string openIdType)
        {
            OpenIdSettingsInfo info = null;
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT * FROM distro_OpenIdSettings WHERE LOWER(OpenIdType)=LOWER(@OpenIdType) AND UserId=@UserId");
            database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType.ToLower());
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }
                OpenIdSettingsInfo info2 = new OpenIdSettingsInfo();
                info2.OpenIdType = openIdType;
                info2.Name = (string)reader["Name"];
                info2.Settings = (string)reader["Settings"];
                info = info2;
                if (reader["Description"] != DBNull.Value)
                {
                    info.Description = (string)reader["Description"];
                }
            }
            return info;
        }

        public override void SaveSettings(OpenIdSettingsInfo settings)
        {
            DbCommand storedProcCommand = database.GetStoredProcCommand("distro_OpenIdSettings_Save");
            database.AddInParameter(storedProcCommand, "OpenIdType", DbType.String, settings.OpenIdType.ToLower());
            database.AddInParameter(storedProcCommand, "Name", DbType.String, settings.Name);
            database.AddInParameter(storedProcCommand, "Description", DbType.String, settings.Description);
            database.AddInParameter(storedProcCommand, "Settings", DbType.String, settings.Settings);
            database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            database.ExecuteNonQuery(storedProcCommand);
        }
    }
}

