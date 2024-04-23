using System.Net;
using Application.TodoItems.Commands;
using Application.TodoItems.Dtos;
using Bogus;
using FluentAssertions;
using Web.IntegrationsTests.Infrastructure;
using Xunit.Abstractions;

namespace Web.IntegrationsTests
{
    public class TodoItemsControllerTests : ApiTestBaseWithDatabase
    {
        protected override string ControllerPath => "/api/todoitems";

        private readonly Faker<CreateTodoItemCommand> _todoItemGenerator = 
            new Faker<CreateTodoItemCommand>()
                .RuleFor(x => x.Title, f => f.Lorem.Word())
                .RuleFor(x => x.Description, f => f.Lorem.Sentence());

        public TodoItemsControllerTests(ITestOutputHelper testOutputHelper, SqlServerTestFixture sqlServerTestFixture) : base(testOutputHelper, sqlServerTestFixture)
        {
        }

        [Fact]
        public async Task Get_ListOfDtos()
        {
            //Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            await InsertTestItem();

            //Act
            var response = await SendGetRequest<List<TodoItemDto>>();

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
            response.Data.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetById_SpecifiedId_OkWithCorrectDto()
        {
            //Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var addedItem = await InsertTestItem();

            //Act
            var response = await SendGetRequest<TodoItemDto>(addedItem!.Id);

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
            response.Data!.Id.Should().Be(addedItem.Id);
        }

        [Fact]
        public async Task GetById_NonExisting_NotFound()
        {
            //Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

            //Act
            var response = await SendGetRequest<TodoItemDto>(1233219938);

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Fact]
        public async Task Create_ValidValues_CreatedDto()
        {
            //Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Created;
            const string expectedTitle = "Test Item for test";

            //Act
            var response = await SendPostRequest<CreateTodoItemCommand, TodoItemDto>(new CreateTodoItemCommand
                { Title = expectedTitle, Description = "Test Description also for test" });

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
            response.Data!.Title.Should().Be(expectedTitle);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("Too Long Title!!, Too Long Title!!, Too Long Title!!, Too Long Title!!, Too Long Title!!, Too Long Title!!, Too Long Title!!")]
        public async Task Create_InvalidTitles_BadRequest(string invalidTitle)
        {
            //Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;

            //Act
            var response = await SendPostRequest<CreateTodoItemCommand, TodoItemDto>(new CreateTodoItemCommand
                { Title = invalidTitle, 
                    Description = "Test Description also for test" });

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Fact]
        public async Task Update_ValidValues_UpdatedDto()
        {
            //Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            const string expectedTitle = "Updated Title";
            var addedItem = await InsertTestItem();

            //Act
            var response = await SendPutRequest<UpdateTodoItemRequestDto, TodoItemDto>(addedItem!.Id, new UpdateTodoItemRequestDto()
            {
                Title = expectedTitle,
                Description = addedItem!.Description
            });

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
            response.Data!.Id.Should().Be(addedItem.Id);
            response.Data.Title.Should().Be(expectedTitle);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("Too Long Title!!, Too Long Title!!, Too Long Title!!, Too Long Title!!, Too Long Title!!, Too Long Title!!, Too Long Title!!")]
        public async Task Update_InvalidTitle_BadRequest(string invalidTitle)
        {
            //Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            var addedItem = await InsertTestItem();

            //Act
            var response = await SendPutRequest<UpdateTodoItemRequestDto, TodoItemDto>(addedItem!.Id, new UpdateTodoItemRequestDto()
            {
                Title = invalidTitle,
                Description = addedItem!.Description
            });

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Fact]
        public async Task Update_NonExisting_NotFound()
        {
            //Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

            //Act
            var response = await SendPutRequest<UpdateTodoItemRequestDto, TodoItemDto>(1928374334, new UpdateTodoItemRequestDto()
            {
                Title = "Test",
                Description = "Not found?"
            });

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Fact]
        public async Task Delete_Existing_NoContent()
        {
            //Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent;
            var addedItem = await InsertTestItem();

            //Act
            var response = await SendDeleteRequest(addedItem!.Id);

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Fact]
        public async Task Delete_NonExisting_NotFound()
        {
            //Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;

            //Act
            var response = await SendDeleteRequest(1928374334);

            //Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }


        private async Task<TodoItemDto?> InsertTestItem()
        {
            return (await SendPostRequest<CreateTodoItemCommand, TodoItemDto>(_todoItemGenerator.Generate())).Data;
        }
    }
}