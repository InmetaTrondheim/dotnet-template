using System.Reflection;
using Domain.ErrorHandling;
using FluentAssertions;

namespace Domain.UnitTests
{
    public class ErrorHandlingTests
    {
        private readonly List<ApiError> _commonErrors;

        public ErrorHandlingTests()
        {
            _commonErrors = PopulateErrors<CommonErrors>();
        }

        [Fact]
        public void CommonErrors_TestDuplicateErrorCodes_AllCodesShouldBeUnique()
        {
            var duplicatesExists = _commonErrors.DistinctBy(e => e.ErrorCode).Count() != _commonErrors.Count();

            duplicatesExists.Should().BeFalse("duplicate codes must not be used");
        }

        [Fact]
        public void CommonErrors_PrefixTest_AllShouldStartWithC()
        {
            var correctPrefix = _commonErrors.All(e => e.ErrorCode.StartsWith("C"));

            correctPrefix.Should().BeTrue("all common errors should be prefixed with C");
        }

        [Fact]
        public void CommonErrors_C0_ExistsWithCorrectStatus()
        {
            var error = _commonErrors.FirstOrDefault(e => e.ErrorCode == "C0");

            error.Should().NotBeNull();
            error.StatusCode.Should().Be(500);
        }

        [Fact]
        public void CommonErrors_C1_ExistsWithCorrectStatus()
        {
            var error = _commonErrors.FirstOrDefault(e => e.ErrorCode == "C1");

            error.Should().NotBeNull();
            error.StatusCode.Should().Be(404);
        }

        [Fact]
        public void CommonErrors_C2_ExistsWithCorrectStatus()
        {
            var error = _commonErrors.FirstOrDefault(e => e.ErrorCode == "C2");

            error.Should().NotBeNull();
            error.StatusCode.Should().Be(403);
        }

        [Fact]
        public void CommonErrors_C3_ExistsWithCorrectStatus()
        {
            var error = _commonErrors.FirstOrDefault(e => e.ErrorCode == "C3");

            error.Should().NotBeNull();
            error.StatusCode.Should().Be(400);
        }

        [Fact]
        public void CommonErrors_C4_ExistsWithCorrectStatus()
        {
            var error = _commonErrors.FirstOrDefault(e => e.ErrorCode == "C4");

            error.Should().NotBeNull();
            error.StatusCode.Should().Be(400);
        }

        private static List<ApiError> PopulateErrors<T>()
        {
            var list = new List<ApiError>();
            var methods = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var method in methods)
            {
                var m = method;

                if (m.ContainsGenericParameters)
                {
                    m = method.MakeGenericMethod(typeof(object));
                }

                var parameterTypes = m.GetParameters().Select(p => p.ParameterType).ToArray();

                var arguments = new object[parameterTypes.Length];

                for (var i = 0; i < parameterTypes.Length; i++)
                {
                    if (parameterTypes[i] == typeof(int))
                    {
                        arguments[i] = 0;
                    }
                    else if (parameterTypes[i] == typeof(string))
                    {
                        arguments[i] = "";
                    }
                    else
                    {
                        arguments[i] = Activator.CreateInstance(parameterTypes[i]);
                    }
                }

                list.Add((ApiError)m.Invoke(null, arguments));
            }

            return list;
        }
    }
}