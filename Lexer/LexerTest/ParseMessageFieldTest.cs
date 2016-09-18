﻿#region Copyright © 2014 Michael Reukauff
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParseMessageFieldTest.cs" company="Michael Reukauff">
//   Copyright © 2014 Michael Reukauff
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace MichaelReukauff.LexerTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Lexer;

    [TestClass]
    public class ParseMessageFieldTest
    {
        [TestMethod]
        public void MessageField_OK01()
        {
            const string text = "  optional string test1 = 1;";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsTrue(lex.ParseMessageField());

            Assert.AreEqual(4, lex.Tokens.Count);
            Assert.AreEqual(0, lex.Errors.Count);
            Assert.AreEqual(6, lex.Index);
        }

        [TestMethod]
        public void MessageField_OK02()
        {
            const string text = "  optional string test1 = 1 [default=\"Farz\"];";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsTrue(lex.ParseMessageField());

            Assert.AreEqual(6, lex.Tokens.Count);
            Assert.AreEqual(0, lex.Errors.Count);
            Assert.AreEqual(13, lex.Index);
        }

        [TestMethod]
        public void MessageField_OK03()
        {
            const string text = "  optional string test1 = 1;\r\naaa";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsTrue(lex.ParseMessageField());

            Assert.AreEqual(4, lex.Tokens.Count);
            Assert.AreEqual(0, lex.Errors.Count);
            Assert.AreEqual(7, lex.Index);
        }

        [TestMethod]
        public void MessageField_OK04()
        {
            const string text = "  optional msg1.msg2.msg3 test1 = 1;\r\naaa";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text)};

            Assert.IsTrue(lex.ParseMessageField());

            Assert.AreEqual(6, lex.Tokens.Count);
            Assert.AreEqual(0, lex.Errors.Count);
            Assert.AreEqual(11, lex.Index);
        }

        [TestMethod]
        public void MessageField_OK05()
        {
            const string text = "  repeated msg1.msg2.msg3 test1 = 1;\r\naaa";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text), Proto3 = true };

            Assert.IsTrue(lex.ParseMessageField());

            Assert.AreEqual(6, lex.Tokens.Count);
            Assert.AreEqual(0, lex.Errors.Count);
            Assert.AreEqual(11, lex.Index);
        }

        [TestMethod]
        public void MessageField_OK06()
        {
            const string text = "  msg1.msg2.msg3 test1 = 1;\r\naaa";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text), Proto3 = true };

            Assert.IsTrue(lex.ParseMessageField());

            Assert.AreEqual(5, lex.Tokens.Count);
            Assert.AreEqual(0, lex.Errors.Count);
            Assert.AreEqual(10, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK01()
        {
            const string text = "  optional string test1 = ;"; // number is missing

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsTrue(lex.ParseMessageField());

            Assert.AreEqual(3, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(5, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK02()
        {
            const string text = "  optional string test1 ;"; // = + number is missing

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(3, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(3, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK03()
        {
            const string text = "  optional string ;"; // statement not finished

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(2, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(2, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK04()
        {
            const string text = "  optional str"; // statement not finished

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(2, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(2, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK05()
        {
            const string text = "  option"; // statement not finished

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(0, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(1, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK06()
        {
            const string text = "  optional test1 = ;"; // statement incomplete

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(2, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(2, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK07()
        {
            const string text = "  optional string test1 = ;\r\naaa"; // number is missing

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsTrue(lex.ParseMessageField());

            Assert.AreEqual(3, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(6, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK08()
        {
            const string text = "  optional";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(1, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(1, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK09()
        {
            const string text = "  optional 1string test1 = 1;";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsTrue(lex.ParseMessageField());

            Assert.AreEqual(4, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(6, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK10()
        {
            const string text = "  optional string test1";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(3, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(3, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK11()
        {
            const string text = "  optional string test1 =";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(3, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(4, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK12()
        {
            const string text = "  optional string test1 = 1";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(4, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(5, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK13()
        {
            const string text = "  optional string test1 = 1 x";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(4, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(5, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK14()
        {
            const string text = "  optional strings.";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(2, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(3, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK15()
        {
            const string text = "  optional string";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text) };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(2, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(2, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK16()
        {
            const string text = "  optional msg1.msg2.msg3 test1 = 1";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text), Proto3 = true };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(0, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(1, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK17()
        {
            const string text = "  repeated ";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text), Proto3 = true };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(1, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(1, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK18()
        {
            const string text = "  optional string test1 = 1;";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text), Proto3 = true };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(0, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(1, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK19()
        {
            const string text = "  required string test1 = 1;";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text), Proto3 = true };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(0, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(1, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK20()
        {
            const string text = "  .";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text), Proto3 = true };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(0, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(1, lex.Index);
        }

        [TestMethod]
        public void MessageField_NOK21()
        {
            const string text = "  .\r\n";

            var lex = new Lexer(text) { Matches = Helper.SplitText(text), Proto3 = true };

            Assert.IsFalse(lex.ParseMessageField());

            Assert.AreEqual(0, lex.Tokens.Count);
            Assert.AreEqual(1, lex.Errors.Count);
            Assert.AreEqual(2, lex.Index);
        }
    }
}
