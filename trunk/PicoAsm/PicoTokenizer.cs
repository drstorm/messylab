/*
 * PicoTokenizer.cs
 *
 * THIS FILE HAS BEEN GENERATED AUTOMATICALLY. DO NOT EDIT!
 */

using System.IO;

using PerCederberg.Grammatica.Runtime;

namespace MessyLab.PicoComputer {

    /**
     * <remarks>A character stream tokenizer.</remarks>
     */
    internal class PicoTokenizer : Tokenizer {

        /**
         * <summary>Creates a new tokenizer for the specified input
         * stream.</summary>
         *
         * <param name='input'>the input stream to read</param>
         *
         * <exception cref='ParserCreationException'>if the tokenizer
         * couldn't be initialized correctly</exception>
         */
        public PicoTokenizer(TextReader input)
            : base(input, true) {

            CreatePatterns();
        }

        /**
         * <summary>Initializes the tokenizer by creating all the token
         * patterns.</summary>
         *
         * <exception cref='ParserCreationException'>if the tokenizer
         * couldn't be initialized correctly</exception>
         */
        private void CreatePatterns() {
            TokenPattern  pattern;

            pattern = new TokenPattern((int) PicoConstants.MOV,
                                       "MOV",
                                       TokenPattern.PatternType.STRING,
                                       "mov");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.ADD,
                                       "ADD",
                                       TokenPattern.PatternType.STRING,
                                       "add");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.SUB,
                                       "SUB",
                                       TokenPattern.PatternType.STRING,
                                       "sub");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.MUL,
                                       "MUL",
                                       TokenPattern.PatternType.STRING,
                                       "mul");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.DIV,
                                       "DIV",
                                       TokenPattern.PatternType.STRING,
                                       "div");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.BEQ,
                                       "BEQ",
                                       TokenPattern.PatternType.STRING,
                                       "beq");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.BGT,
                                       "BGT",
                                       TokenPattern.PatternType.STRING,
                                       "bgt");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.IN,
                                       "IN",
                                       TokenPattern.PatternType.STRING,
                                       "in");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.OUT,
                                       "OUT",
                                       TokenPattern.PatternType.STRING,
                                       "out");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.JSR,
                                       "JSR",
                                       TokenPattern.PatternType.STRING,
                                       "jsr");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.RTS,
                                       "RTS",
                                       TokenPattern.PatternType.STRING,
                                       "rts");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.STOP,
                                       "STOP",
                                       TokenPattern.PatternType.STRING,
                                       "stop");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.ORG,
                                       "ORG",
                                       TokenPattern.PatternType.STRING,
                                       "org");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.EQUALS,
                                       "EQUALS",
                                       TokenPattern.PatternType.STRING,
                                       "=");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.LEFT_PAREN,
                                       "LEFT_PAREN",
                                       TokenPattern.PatternType.STRING,
                                       "(");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.RIGHT_PAREN,
                                       "RIGHT_PAREN",
                                       TokenPattern.PatternType.STRING,
                                       ")");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.HASH,
                                       "HASH",
                                       TokenPattern.PatternType.STRING,
                                       "#");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.COLON,
                                       "COLON",
                                       TokenPattern.PatternType.STRING,
                                       ":");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.COMMA,
                                       "COMMA",
                                       TokenPattern.PatternType.STRING,
                                       ",");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.SIGN,
                                       "SIGN",
                                       TokenPattern.PatternType.REGEXP,
                                       "[+-]");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.NUMBER,
                                       "NUMBER",
                                       TokenPattern.PatternType.REGEXP,
                                       "[0-9]+");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.IDENTIFIER,
                                       "IDENTIFIER",
                                       TokenPattern.PatternType.REGEXP,
                                       "[a-z][a-z0-9_]*");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.ENTER,
                                       "ENTER",
                                       TokenPattern.PatternType.REGEXP,
                                       "[\\n\\r]+");
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.SINGLE_LINE_COMMENT,
                                       "SINGLE_LINE_COMMENT",
                                       TokenPattern.PatternType.REGEXP,
                                       ";.*");
            pattern.Ignore = true;
            AddPattern(pattern);

            pattern = new TokenPattern((int) PicoConstants.WHITESPACE,
                                       "WHITESPACE",
                                       TokenPattern.PatternType.REGEXP,
                                       "[ \\t]+");
            pattern.Ignore = true;
            AddPattern(pattern);
        }
    }
}
