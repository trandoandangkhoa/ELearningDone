using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.BookingCalender;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.BookingCalender
{
    public class AppointmentMappingProfile : Profile
    {
        public AppointmentMappingProfile() 
        {
            CreateMap<AppointmentSlot, AppointmentSlotDto>().ForSourceMember(dest => dest.Room, opt => opt.DoNotValidate()).IgnoreNoMap();


        }
        
    }
    public class NoMapAttribute : System.Attribute
    {
    }
    public static class IgnoreNoMapExtensions
    {
        //Method is Generic and Hence we can use with any TSource and TDestination Type
        public static IMappingExpression<TSource, TDestination> IgnoreNoMap<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression)
        {
            //Fetching Type of the TSource
            var sourceType = typeof(TSource);
            //Fetching All Properties of the Source Type using GetProperties() method
            foreach (var property in sourceType.GetProperties())
            {
                //Get the Property Name
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(sourceType)[property.Name];
                //Check if Property is Decorated with the NoMapAttribute
                NoMapAttribute attribute = (NoMapAttribute)descriptor.Attributes[typeof(NoMapAttribute)];
                if (attribute != null)
                {
                    //If Property is Decorated with NoMap Attribute, call the Ignore Method
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }
    }
}
