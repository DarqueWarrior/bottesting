using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.QualityTools.Testing.Fakes;
using MyBot.Dialogs;

namespace MyBot.Tests.Controllers
{
   [TestClass]
   public class MessagesControllerTests
   {
      [TestMethod]
      public async Task Returns_HttpStatusCode_StartsRootDialog()
      {
         using (ShimsContext.Create())
         {
            // Arrange
            var sendAsyncCalled = false;
            var isCorrectDialog = false;
            var activity = new Activity(ActivityTypes.Message);

            var target = new MessagesController()
            {
               Request = new System.Net.Http.HttpRequestMessage()
            };

            // Fake the call to Conversation.SendAsync
            Microsoft.Bot.Builder.Dialogs.Fakes.ShimConversation.SendAsyncIMessageActivityFuncOfIDialogOfObjectCancellationToken = (a, d, t) =>
            {
               sendAsyncCalled = true;

               // See if the type of dialog is correct
               isCorrectDialog = d() is RootDialog;

               return Task.CompletedTask;
            };

            // Act
            var task = await target.Post(activity);

            // Assert
            Assert.IsTrue(sendAsyncCalled, "SendAsync was not called");
            Assert.IsTrue(isCorrectDialog, "The wrong dialog was provided");
            Assert.AreEqual(System.Net.HttpStatusCode.OK, task.StatusCode, "The wrong status code was returned");
         }
      }

      [TestMethod]
      public async Task Returns_HttpStatusCode_DeleteUserData()
      {
         // Arrange
         var target = new MessagesController()
         {
            Request = new System.Net.Http.HttpRequestMessage()
         };

         // Act
         var activity = new Activity(ActivityTypes.DeleteUserData);
         var task = await target.Post(activity);

         // Assert
         Assert.AreEqual(System.Net.HttpStatusCode.OK, task.StatusCode);
      }

      [TestMethod]
      public async Task Returns_HttpStatusCode_ContactRelationUpdate()
      {
         // Arrange
         var target = new MessagesController()
         {
            Request = new System.Net.Http.HttpRequestMessage()
         };

         // Act
         var activity = new Activity(ActivityTypes.ContactRelationUpdate);
         var task = await target.Post(activity);

         // Assert
         Assert.AreEqual(System.Net.HttpStatusCode.OK, task.StatusCode);
      }

      [TestMethod]
      public async Task Returns_HttpStatusCode_ConversationUpdate()
      {
         // Arrange
         var target = new MessagesController()
         {
            Request = new System.Net.Http.HttpRequestMessage()
         };

         // Act
         var activity = new Activity(ActivityTypes.ConversationUpdate);
         var task = await target.Post(activity);

         // Assert
         Assert.AreEqual(System.Net.HttpStatusCode.OK, task.StatusCode);
      }

      [TestMethod]
      public async Task Returns_HttpStatusCode_Typing()
      {
         // Arrange
         var target = new MessagesController()
         {
            Request = new System.Net.Http.HttpRequestMessage()
         };

         // Act
         var activity = new Activity(ActivityTypes.Typing);
         var task = await target.Post(activity);

         // Assert
         Assert.AreEqual(System.Net.HttpStatusCode.OK, task.StatusCode);
      }

      [TestMethod]
      public async Task Returns_HttpStatusCode_Ping()
      {
         // Arrange
         var target = new MessagesController()
         {
            Request = new System.Net.Http.HttpRequestMessage()
         };

         // Act
         var activity = new Activity(ActivityTypes.Ping);
         var task = await target.Post(activity);

         // Assert
         Assert.AreEqual(System.Net.HttpStatusCode.OK, task.StatusCode);
      }
   }
}
