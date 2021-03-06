﻿namespace Hidistro.Entities.Members
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class DistributorGradeInfo
    {
        
       string _Description ;
        
       int _Discount ;
        
       int _GradeId ;
        
       string _Name ;

        [HtmlCoding, StringLengthValidator(0, 300, Ruleset="ValDistributorGrade", MessageTemplate="备注的长度限制在300个字符以内")]
        public string Description
        {
            
            get
            {
                return this._Description ;
            }
            
            set
            {
                this._Description  = value;
            }
        }

        [RangeValidator(typeof(int), "1", RangeBoundaryType.Inclusive, "100", RangeBoundaryType.Inclusive, Ruleset="ValDistributorGrade", MessageTemplate="等级折扣必须在1-100之间")]
        public int Discount
        {
            
            get
            {
                return this._Discount ;
            }
            
            set
            {
                this._Discount  = value;
            }
        }

        public int GradeId
        {
            
            get
            {
                return this._GradeId ;
            }
            
            set
            {
                this._GradeId  = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValDistributorGrade", MessageTemplate="分销商等级名称不能为空，长度限制在60个字符以内"), HtmlCoding]
        public string Name
        {
            
            get
            {
                return this._Name ;
            }
            
            set
            {
                this._Name  = value;
            }
        }
    }
}

