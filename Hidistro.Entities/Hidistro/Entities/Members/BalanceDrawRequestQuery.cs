﻿namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class BalanceDrawRequestQuery : Pagination
    {
        
       DateTime? _FromDate ;
        
       DateTime? _ToDate ;
        
       int? _UserId ;
        
       string _UserName ;

        public DateTime? FromDate
        {
            
            get
            {
                return this._FromDate ;
            }
            
            set
            {
                this._FromDate  = value;
            }
        }

        public DateTime? ToDate
        {
            
            get
            {
                return this._ToDate ;
            }
            
            set
            {
                this._ToDate  = value;
            }
        }

        public int? UserId
        {
            
            get
            {
                return this._UserId ;
            }
            
            set
            {
                this._UserId  = value;
            }
        }

        public string UserName
        {
            
            get
            {
                return this._UserName ;
            }
            
            set
            {
                this._UserName  = value;
            }
        }
    }
}

