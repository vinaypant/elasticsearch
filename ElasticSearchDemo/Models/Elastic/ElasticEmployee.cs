using System;
using System.Collections.Generic;
using Nest;
namespace ElasticSearchDemo.Models.Elastic
{
    
    [ElasticsearchType(IdProperty ="Id",RelationName = "employee")]
    public class ElasticEmployee
    {
        [Text(Name ="id")]
        public Guid? Id { get; set; }

        [Text(Name="name")]
        public string Name { get; set; }

        [Number(Name="age")]
        public int Age { get; set; }

        [Date(Name ="dob",Format ="MM/dd/yyyy",IgnoreMalformed=true)]
        public DateTime DateOfBirth { get; set; }

        [Boolean(Name ="is_permanent",NullValue =true,Store =true)]
        public bool IsPermanent { get; set; }

        [Object]
        public List<ElasticSkill> Skills { get; set; }

        [Nested]
        [PropertyName("empl")]
        public List<ElasticEmployee> Employees { get; set; }

        [Number(NumberType.Double,IgnoreMalformed =true,Coerce =true)]
        //[Text]
        public int Salary { get; set; }

        [Keyword(Name ="email")]
        public string Email { get; set; }

    }
    public class ElasticSkill
    {
        [Text(Name ="skill_name")]
        public string Name { get; set; }

        [Number(NumberType.Byte,Name = "level")]
        public int Proficiency { get; set; }
    }
}
