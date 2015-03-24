﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Silverpop.Core.Tests
{
    public class TransactMessageRecipientTests
    {
        public class CreateTests
        {
            [Fact]
            public void ThrowsForNullEmailAddress()
            {
                Assert.Throws<ArgumentNullException>(
                    () => TransactMessageRecipient.Create(null));
            }

            [Fact]
            public void SetsEmailAddress()
            {
                var recipient = TransactMessageRecipient.Create(
                    "test@example.com");

                Assert.Equal("test@example.com", recipient.EmailAddress);
            }

            [Fact]
            public void SetsBodyType()
            {
                var recipient = TransactMessageRecipient.Create(
                    "test@example.com",
                    bodyType: TransactMessageRecipientBodyType.Text);

                Assert.Equal(TransactMessageRecipientBodyType.Text, recipient.BodyType);
            }
        }

        public class CreateOfTTests
        {
            [Fact]
            public void ThrowsForNullEmailAddress()
            {
                Assert.Throws<ArgumentNullException>(
                    () => TransactMessageRecipient.Create<object>(null, new { }));
            }

            [Fact]
            public void ThrowsForNullPersonalizationTagsObject()
            {
                Assert.Throws<ArgumentNullException>(
                    () => TransactMessageRecipient.Create<object>("test@example.com", null));
            }

            [Fact]
            public void SetsEmailAddress()
            {
                var recipient = TransactMessageRecipient.Create<object>(
                    "test@example.com",
                    new { });

                Assert.Equal("test@example.com", recipient.EmailAddress);
            }

            [Fact]
            public void SetsBodyType()
            {
                var recipient = TransactMessageRecipient.Create<object>(
                    "test@example.com",
                    new { },
                    bodyType: TransactMessageRecipientBodyType.Text);

                Assert.Equal(TransactMessageRecipientBodyType.Text, recipient.BodyType);
            }

            [Fact]
            public void SetsPersonalizationTags()
            {
                var recipient = TransactMessageRecipient.Create<object>(
                    "test@example.com",
                    new
                    {
                        Tag1 = "tag1-value",
                        Tag2 = "tag2-value"
                    });

                Assert.Equal(2, recipient.PersonalizationTags.Count());

                Assert.Equal("Tag1", recipient.PersonalizationTags.First().Name);
                Assert.Equal("tag1-value", recipient.PersonalizationTags.First().Value);

                Assert.Equal("Tag2", recipient.PersonalizationTags.Last().Name);
                Assert.Equal("tag2-value", recipient.PersonalizationTags.Last().Value);
            }

            [Fact]
            public void SetsPersonalizationTagsUsingSilverpopPersonalizationTagName()
            {
                var recipient = TransactMessageRecipient.Create<TestPersonalizationTagsWithSilverpopPersonalizationTag>(
                    "test@example.com",
                    new TestPersonalizationTagsWithSilverpopPersonalizationTag()
                    {
                        Tag1 = "tag1-value",
                        Tag2 = "tag2-value"
                    });

                Assert.Equal(2, recipient.PersonalizationTags.Count());

                Assert.Equal("special-tag1-name", recipient.PersonalizationTags.First().Name);
                Assert.Equal("tag1-value", recipient.PersonalizationTags.First().Value);

                Assert.Equal("Tag2", recipient.PersonalizationTags.Last().Name);
                Assert.Equal("tag2-value", recipient.PersonalizationTags.Last().Value);
            }

            [Fact]
            public void ShouldNotThrowWhenPersonalizationTagsDictionaryHasANullValue()
            {
                Assert.DoesNotThrow(
                    () => new TransactMessageRecipient()
                    {
                        EmailAddress = "test@example.com",
                        PersonalizationTags = new List<TransactMessageRecipientPersonalizationTag>()
                        {
                            new TransactMessageRecipientPersonalizationTag("Tag1", null)
                        }
                    });
            }

            [Fact]
            public void ShouldNotThrowWhenPropertyInPersonalizationTagsObjectHasANullValue()
            {
                // Verify this test is valid when using the
                // TestPersonalizationTagsWithSilverpopPersonalizationTag type.
                var propertiesCount = new TestPersonalizationTagsWithSilverpopPersonalizationTag()
                    .GetType()
                    .GetProperties(TransactMessageRecipient.DefaultPersonalizationTagsPropertyReflectionBindingFlags)
                    .Count();

                Assert.True(propertiesCount > 1);

                Assert.DoesNotThrow(
                    () => TransactMessageRecipient.Create<TestPersonalizationTagsWithSilverpopPersonalizationTag>(
                        "test@example.com",
                        new TestPersonalizationTagsWithSilverpopPersonalizationTag()
                        {
                            Tag1 = "tag1-value",
                        }));
            }

            public class TestPersonalizationTagsWithSilverpopPersonalizationTag
            {
                [SilverpopPersonalizationTag("special-tag1-name")]
                public string Tag1 { get; set; }

                public string Tag2 { get; set; }
            }
        }
    }
}