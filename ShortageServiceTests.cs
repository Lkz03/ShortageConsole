using UnitTests.Services;
using Visma_task.Constants;
using Visma_task.Enums;
using Visma_task.Helpers;

namespace UnitTests
{
    public class ShortageServiceTests
    {
        private ShortageService _service;

        [SetUp]
        public void Setup()
        {
            if (!Directory.Exists(Paths.WritePath))
            {
                Directory.CreateDirectory(Paths.WritePath);
            }

            _service = new ShortageService();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(Paths.ShortageFilePath))
            {
                File.Delete(Paths.ShortageFilePath);
            }
        }

        [Test]
        public void AddShortage_WithValidInput_AddsShortageSuccessfully()
        {
            // Arrange
            var title = "AddSuccess";
            var room = Room.Bathroom;

            _service.AddShortage(title, room);

            // Act
            var shortages = ShortageJsonHelper.ReadShortages();
            bool result = shortages.ContainsKey($"{title}-{room}");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void RemoveShortage_AsAdmin_RemovesShortageSuccessfully()
        {
            // Arrange
            var title = "delete";
            var room = Room.Bathroom;

            _service.AddShortage(title, room);

            // Act
            bool removeResult = _service.RemoveShortage(title, room, true);

            // Assert
            var shortages = ShortageJsonHelper.ReadShortages();
            Assert.That(removeResult, Is.True);
            Assert.That(shortages.ContainsKey($"{title}-{room}"), Is.False);
        }
    }
}