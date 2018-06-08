using FieldScribeAPI;
using FieldScribeAPI.Controllers;
using FieldScribeAPI.Models;
using FieldScribeAPI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading;
using NUnit;
//using Xunit;

namespace UnitTests
{
    // Simple test to see if unit tests are working

    [TestClass]
    public class Class1
    {
        [TestMethod]
        public void PassingTest()
        {
            Assert.AreEqual(4, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }

    // Not working yet

    //[TestClass]
    //public class AthletesControllerTest
    //{
    //    protected AthletesController TestAthletesController { get; set; }
    //    protected Mock<IAthleteService> AthleteServiceMock { get; set; }

    //    protected PagingOptions _pagingOptions { get; set; }

    //    protected SortOptions<Athlete, AthleteEntity> _sortOptions {get;set;}

    //    protected SearchOptions<Athlete, AthleteEntity> _searchOptions { get; set; }

    //    protected CancellationToken _ct { get; set; }

    //    [TestInitialize]
    //    public void AthletesTest(IOptions<PagingOptions> pagingOptions,
    //        PagingOptions po,
    //        SortOptions<Athlete, AthleteEntity> sortOptions,
    //        SearchOptions<Athlete, AthleteEntity> searchOptions,
    //        CancellationToken ct)
    //    {
    //        AthleteServiceMock = new Mock<IAthleteService>();
    //        TestAthletesController = new AthletesController(AthleteServiceMock.Object, pagingOptions);

    //        _pagingOptions = po;
    //        _sortOptions = sortOptions;
    //        _searchOptions = searchOptions;
    //        _ct = ct;
    //    }

    //    [TestMethod]
    //    public async Task Return_All_Athletes()
    //    {
    //        var expectedAthletes = new PagedResults<Athlete>
    //        {
    //            Items = new List<Athlete>
    //                {
    //                    new Athlete {Events = null, athleteId = 1, meetId = 1, CompNum = 1,
    //                    FirstName = "Willy", LastName = "Wonka", TeamName = "Chocolate Factory", Gender = "M"},
    //                    new Athlete {Events = null, athleteId = 2, meetId = 1, CompNum = 2,
    //                    FirstName = "Jesse", LastName = "James", TeamName = "Outlaws", Gender = "M"},
    //                    new Athlete {Events = null, athleteId = 3, meetId = 1, CompNum = 3,
    //                    FirstName = "Mickey", LastName = "Mouse", TeamName = "Disney", Gender = "M"},
    //                },

    //            TotalSize = 3
    //        };

    //        AthleteServiceMock
    //            .Setup(x => x.GetAthletesAsync(_pagingOptions, _sortOptions, _searchOptions, _ct))
    //            .ReturnsAsync(expectedAthletes);

    //        var result = await TestAthletesController.GetAthletesAsync(
    //            _pagingOptions, _sortOptions, _searchOptions, _ct);

    //        Assert.AreSame(expectedAthletes, result);
    //    }
    //}
}