/*
 * PicoParser.cs
 *
 * THIS FILE HAS BEEN GENERATED AUTOMATICALLY. DO NOT EDIT!
 */

using System.IO;

using PerCederberg.Grammatica.Runtime;

namespace MessyLab.PicoComputer {

    /**
     * <remarks>A token stream parser.</remarks>
     */
    internal class PicoParser : RecursiveDescentParser {

        /**
         * <summary>An enumeration with the generated production node
         * identity constants.</summary>
         */
        private enum SynteticPatterns {
            SUBPRODUCTION_1 = 3001,
            SUBPRODUCTION_2 = 3002,
            SUBPRODUCTION_3 = 3003,
            SUBPRODUCTION_4 = 3004,
            SUBPRODUCTION_5 = 3005,
            SUBPRODUCTION_6 = 3006,
            SUBPRODUCTION_7 = 3007,
            SUBPRODUCTION_8 = 3008,
            SUBPRODUCTION_9 = 3009,
            SUBPRODUCTION_10 = 3010,
            SUBPRODUCTION_11 = 3011
        }

        /**
         * <summary>Creates a new parser with a default analyzer.</summary>
         *
         * <param name='input'>the input stream to read from</param>
         *
         * <exception cref='ParserCreationException'>if the parser
         * couldn't be initialized correctly</exception>
         */
        public PicoParser(TextReader input)
            : base(input) {

            CreatePatterns();
        }

        /**
         * <summary>Creates a new parser.</summary>
         *
         * <param name='input'>the input stream to read from</param>
         *
         * <param name='analyzer'>the analyzer to parse with</param>
         *
         * <exception cref='ParserCreationException'>if the parser
         * couldn't be initialized correctly</exception>
         */
        public PicoParser(TextReader input, PicoAnalyzer analyzer)
            : base(input, analyzer) {

            CreatePatterns();
        }

        /**
         * <summary>Creates a new tokenizer for this parser. Can be overridden
         * by a subclass to provide a custom implementation.</summary>
         *
         * <param name='input'>the input stream to read from</param>
         *
         * <returns>the tokenizer created</returns>
         *
         * <exception cref='ParserCreationException'>if the tokenizer
         * couldn't be initialized correctly</exception>
         */
        protected override Tokenizer NewTokenizer(TextReader input) {
            return new PicoTokenizer(input);
        }

        /**
         * <summary>Initializes the parser by creating all the production
         * patterns.</summary>
         *
         * <exception cref='ParserCreationException'>if the parser
         * couldn't be initialized correctly</exception>
         */
        private void CreatePatterns() {
            ProductionPattern             pattern;
            ProductionPatternAlternative  alt;

            pattern = new ProductionPattern((int) PicoConstants.PROGRAM,
                                            "Program");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.SEPARATOR, 0, 1);
            alt.AddProduction((int) PicoConstants.SYMBOLS, 1, 1);
            alt.AddProduction((int) PicoConstants.ORIGIN, 1, 1);
            alt.AddProduction((int) PicoConstants.LINES, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.SEPARATOR,
                                            "Separator");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.ENTER, 1, 1);
            alt.AddToken((int) PicoConstants.ENTER, 0, -1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.SYMBOLS,
                                            "Symbols");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.SYMBOL, 1, 1);
            alt.AddProduction((int) PicoConstants.SYMBOL, 0, -1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.SYMBOL,
                                            "Symbol");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.IDENTIFIER, 1, 1);
            alt.AddToken((int) PicoConstants.EQUALS, 1, 1);
            alt.AddProduction((int) PicoConstants.INTEGER, 1, 1);
            alt.AddProduction((int) PicoConstants.SEPARATOR, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.INTEGER,
                                            "Integer");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.SIGN, 0, 1);
            alt.AddToken((int) PicoConstants.NUMBER, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.ORIGIN,
                                            "Origin");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.ORG, 1, 1);
            alt.AddToken((int) PicoConstants.NUMBER, 1, 1);
            alt.AddProduction((int) PicoConstants.SEPARATOR, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.LINES,
                                            "Lines");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.LINE, 1, 1);
            alt.AddProduction((int) PicoConstants.LINE, 0, -1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.LINE,
                                            "Line");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_1, 0, 1);
            alt.AddProduction((int) PicoConstants.INSTRUCTION, 1, 1);
            alt.AddProduction((int) PicoConstants.SEPARATOR, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.INSTRUCTION,
                                            "Instruction");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.MOVE, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARITHMETIC, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.BRANCH, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.IO, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.CALL, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.RETURN, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.END, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.MOVE,
                                            "Move");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.MOV, 1, 1);
            alt.AddProduction((int) PicoConstants.MOVE_ARGS, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.ARITHMETIC,
                                            "Arithmetic");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_2, 1, 1);
            alt.AddProduction((int) PicoConstants.ARITHMETIC_ARGS, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.BRANCH,
                                            "Branch");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_3, 1, 1);
            alt.AddProduction((int) PicoConstants.BRANCH_ARGS, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.IO,
                                            "IO");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_4, 1, 1);
            alt.AddProduction((int) PicoConstants.IOARGS, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.CALL,
                                            "Call");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.JSR, 1, 1);
            alt.AddProduction((int) PicoConstants.ARG3, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.RETURN,
                                            "Return");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.RTS, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.END,
                                            "End");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.STOP, 1, 1);
            alt.AddProduction((int) PicoConstants.END_ARGS, 0, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.MOVE_ARGS,
                                            "MoveArgs");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG34, 1, 1);
            alt.AddToken((int) PicoConstants.COMMA, 1, 1);
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_5, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.ARITHMETIC_ARGS,
                                            "ArithmeticArgs");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG34, 1, 1);
            alt.AddToken((int) PicoConstants.COMMA, 1, 1);
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_6, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.BRANCH_ARGS,
                                            "BranchArgs");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_8, 1, 1);
            alt.AddToken((int) PicoConstants.COMMA, 1, 1);
            alt.AddProduction((int) PicoConstants.ARG34, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.IOARGS,
                                            "IOArgs");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG34, 1, 1);
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_9, 0, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.END_ARGS,
                                            "EndArgs");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG34, 1, 1);
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_11, 0, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.ARG12,
                                            "Arg12");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG1, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG2, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.ARG34,
                                            "Arg34");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG3, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG4, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.ARG123,
                                            "Arg123");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG1, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG2, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG3, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.ARG1234,
                                            "Arg1234");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG1, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG2, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG3, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG4, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.ARG1,
                                            "Arg1");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.INTEGER, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.ARG2,
                                            "Arg2");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.HASH, 1, 1);
            alt.AddToken((int) PicoConstants.IDENTIFIER, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.ARG3,
                                            "Arg3");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.IDENTIFIER, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) PicoConstants.ARG4,
                                            "Arg4");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.LEFT_PAREN, 1, 1);
            alt.AddToken((int) PicoConstants.IDENTIFIER, 1, 1);
            alt.AddToken((int) PicoConstants.RIGHT_PAREN, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_1,
                                            "Subproduction1");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.IDENTIFIER, 1, 1);
            alt.AddToken((int) PicoConstants.COLON, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_2,
                                            "Subproduction2");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.ADD, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.SUB, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.MUL, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.DIV, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_3,
                                            "Subproduction3");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.BEQ, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.BGT, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_4,
                                            "Subproduction4");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.IN, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.OUT, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_5,
                                            "Subproduction5");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG1234, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG34, 1, 1);
            alt.AddToken((int) PicoConstants.COMMA, 1, 1);
            alt.AddProduction((int) PicoConstants.ARG123, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_6,
                                            "Subproduction6");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG34, 1, 1);
            alt.AddToken((int) PicoConstants.COMMA, 1, 1);
            alt.AddProduction((int) PicoConstants.ARG1234, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG12, 1, 1);
            alt.AddToken((int) PicoConstants.COMMA, 1, 1);
            alt.AddProduction((int) PicoConstants.ARG34, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_7,
                                            "Subproduction7");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG34, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG1, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_8,
                                            "Subproduction8");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG34, 1, 1);
            alt.AddToken((int) PicoConstants.COMMA, 1, 1);
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_7, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) PicoConstants.ARG1, 1, 1);
            alt.AddToken((int) PicoConstants.COMMA, 1, 1);
            alt.AddProduction((int) PicoConstants.ARG34, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_9,
                                            "Subproduction9");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.COMMA, 1, 1);
            alt.AddProduction((int) PicoConstants.ARG1234, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_10,
                                            "Subproduction10");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.COMMA, 1, 1);
            alt.AddProduction((int) PicoConstants.ARG34, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) SynteticPatterns.SUBPRODUCTION_11,
                                            "Subproduction11");
            pattern.Synthetic = true;
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) PicoConstants.COMMA, 1, 1);
            alt.AddProduction((int) PicoConstants.ARG34, 1, 1);
            alt.AddProduction((int) SynteticPatterns.SUBPRODUCTION_10, 0, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);
        }
    }
}
