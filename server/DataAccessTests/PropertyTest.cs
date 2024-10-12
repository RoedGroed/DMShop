using DataAccess;
using PgCtx;
using SharedTestDependencies;
using Xunit;


namespace DataAccessTests
{
    public class PropertyTest
    {
        private PgCtxSetup<DmShopContext> _pgCtxSetup = new();

        [Fact]
        public void GetPropertiesForList_ReturnsCorrectProperties()
        {
            // Arrange
            var properties = TestObjects.GetProperties(3); 
            _pgCtxSetup.DbContextInstance.Properties.AddRange(properties);
            _pgCtxSetup.DbContextInstance.SaveChanges();

            // Act
            var result = new DmShopRepository(_pgCtxSetup.DbContextInstance).GetAllProperties();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, p => p.PropertyName == properties[0].PropertyName);
            Assert.Contains(result, p => p.PropertyName == properties[1].PropertyName);
            Assert.Contains(result, p => p.PropertyName == properties[2].PropertyName);
        }
    }
}