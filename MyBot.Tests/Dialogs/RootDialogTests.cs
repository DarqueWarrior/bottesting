using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using MyBot.Dialogs;
using Microsoft.Bot.Connector;

namespace Blog.Tests.Dialogs
{
   [TestClass]
   public class RootDialogTests
   {
      [TestMethod]
      public async Task MessageReceivedAsync_HelloWorld()
      {
         using (ShimsContext.Create())
         {
            // Arrange
            var waitCalled = false;
            var message = string.Empty;
            var postAsyncCalled = false;

            var target = new RootDialog();

            var activity = new Activity(ActivityTypes.Message)
            {
               Text = "Hello World"
            };

            var awaiter = new Microsoft.Bot.Builder.Internals.Fibers.Fakes.StubIAwaiter<IMessageActivity>()
            {
               IsCompletedGet = () => true,
               GetResult = () => activity
            };

            var awaitable = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIAwaitable<IMessageActivity>()
            {
               GetAwaiter = () => awaiter
            };

            var context = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIDialogContext();

            Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.PostAsyncIBotToUserStringStringCancellationToken = (user, s1, s2, token) =>
            {
               message = s1;
               postAsyncCalled = true;
               return Task.CompletedTask;
            };

            Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.WaitIDialogStackResumeAfterOfIMessageActivity = (stack, callback) =>
            {
               if (waitCalled) return;

               waitCalled = true;

               // The callback is what is being tested.
               callback(context, awaitable);
            };

            // Act
            await target.StartAsync(context);

            // Assert
            Assert.AreEqual("You sent Hello World which was 11 characters", message, "Message is wrong");
            Assert.IsTrue(postAsyncCalled, "PostAsync was not called");
         }
      }

      [TestMethod]
      public async Task MessageReceivedAsync_EmptyString()
      {
         using (ShimsContext.Create())
         {
            // Arrange
            var waitCalled = false;
            var message = string.Empty;
            var postAsyncCalled = false;

            var target = new RootDialog();
            var activity = new Activity(ActivityTypes.Message);

            var awaiter = new Microsoft.Bot.Builder.Internals.Fibers.Fakes.StubIAwaiter<IMessageActivity>()
            {
               IsCompletedGet = () => true,
               GetResult = () => activity
            };

            var awaitable = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIAwaitable<IMessageActivity>()
            {
               GetAwaiter = () => awaiter
            };

            var context = new Microsoft.Bot.Builder.Dialogs.Fakes.StubIDialogContext();
            Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.PostAsyncIBotToUserStringStringCancellationToken = (user, s1, s2, token) =>
            {
               postAsyncCalled = true;
               message = s1;
               return Task.CompletedTask;
            };

            Microsoft.Bot.Builder.Dialogs.Fakes.ShimExtensions.WaitIDialogStackResumeAfterOfIMessageActivity = (stack, callback) =>
            {
               if (waitCalled) return;

               waitCalled = true;

               // The callback is what is being tested.
               callback(context, awaitable);
            };

            // Act
            await target.StartAsync(context);

            // Assert
            Assert.AreEqual("You sent  which was 0 characters", message, "Message is wrong");
            Assert.IsTrue(postAsyncCalled, "PostAsync was not called");
         }
      }
   }
}
