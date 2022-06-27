﻿using NUnit.Framework;

using AutoFixture.AutoNSubstitute;
using AutoFixture;
using System.Collections.Generic;
using NSubstitute;
using System.IO.Abstractions;

namespace Migration.Common.Tests
{
    [TestFixture]
    public class MigrationCommonTests
    {
        // use auto fixiture to help mock and instantiate with dummy data with nsubsitute. 
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoNSubstituteCustomization() { });
        }

        [Test]
        public void When_generating_user_map_Then_map_is_correct()
        {
            string[] userMapLines = { "a@jira.com=a@azdo.com", "b@jira.com=b@azdo.com" };
            Dictionary<string, string> generatedUserMap = UserMapper.ParseUserMappings(userMapLines);

            foreach(string line in userMapLines)
            {
                string[] splitLine = line.Split("=");
                string source = splitLine[0];
                string target = splitLine[1];

                Assert.Contains(source, generatedUserMap.Keys);
                Assert.AreEqual(target, generatedUserMap[source]);
            }
        }

        [Test]
        public void When_calling_ParseUserMappings_with_non_exisiting_file_Return_empty_Dictionary()
        {
            //Assign

            var fileSystem = _fixture.Freeze<IFileSystem>();
            fileSystem.File.Exists(Arg.Any<string>()).Returns(true);

            var expected = new Dictionary<string, string>();

            //Act
            var actualResult = UserMapper.ParseUserMappings(Arg.Any<string>());

            //Assert
            Assert.That(actualResult.Count, Is.EqualTo(expected.Count));
        }

    }
}