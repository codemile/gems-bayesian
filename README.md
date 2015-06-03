# Overview

gems-bayesian is a C# library for a Bayesian probability filter that can be used to categorize text into good or bad collections.

## What is Bayesian filtering?

Bayesian filtering is a popular statistical technique used for e-mail filtering to detect spam. If you separated a collection of emails into two different piles. One pile containing spam and the other containing non-spam, then you can calculate the statistical likely hood of a word belonging to the spam or non-spam pile. Once the statistical look-up table is created it can be used to categorize future emails.

While Bayesian filtering was popularized for use in spam detection it has many other usages outside of email.

# Usage

gems-bayesian is a C# library you can include with any Microsoft .NET compatible project. The library is easy to use, small and can be installed using NuGet packaging.

To get started you need the following.

- a collection of good words from text you want to keep
- a collection of bad words from text you want to reject

> Note: It's normal for words to exist in both the good and bad collections. This helps tell the filter that these words should be ignored. Words like "the", "this" and "that" as an example. 

## Creating good and bad collections

The first thing you need to do is create two lists of words. These lists will be used as the good and bad collections for the statistical table. Where you get the `words` from is up to you.

    using Bayesian;

    TokenCollection good = new TokenCollection();
    foreach(string word in good_words) {
        good.Add(word);
    }

    TokenCollection bad = new TokenCollection();
    foreach(string word in bad_words) {
        bad.Add(word);
    }

> Note: It's important that the list of words you add to a `TokenCollection` contains a unique list of words. Any duplicates would cause unexpected results in the statistical table data.

## Reading a document

gems-bayesian handles text documents as a stream of `Tokens`. Each token represents a word in the document and these tokens are analyzed using the good and bad collections.

Use the `Tokens` class to store the stream, and a `Processors` class to convert the text into tokens.

Here is an example for creating a `Tokens` object.

    using System.IO;
    using Bayesian;
    using Bayesian.Words;

    // read a text file
    string document = File.ReadAllText("your_text_file.txt");

    // create a processor for the tokens.
    Processors proc = new Processors();

    // convert the text file into a collection of tokens using the processor.
    Tokens tokens = new Tokens(document, proc);

    // score tells which collection does the document belongs in
    float score = Analyzer.score(tokens, good, bad);
    
## Analyzing the document

The process of *analyzing* a document means to calculate the statistical likely hood that the document belongs to the same population of words that either the good or bad words came from.

You use the `Analyzer` class to score the `Tokens` against either the good or bad `TokenCollection` objects. The static class contains a single method:

    static float Score(IEnumerable<string> pTokens, TokenCollection pGood, TokenCollection pBad);

This method returns a float value ranging from `0` to `1` that represents which collection the document belongs in. Where a value of `1` means that it belongs to the good collection, and a value of `0` means that it belongs to the bad collection. The middle value of `0.5` infers that the document weighs equally in both collections (or another way to say that the analyzer can't tell the difference). 

# License

gems-bayesian is released under the MIT license. See [LICENSE](https://github.com/thinkingmedia/gems-bayesian/blob/master/LICENSE) for defaults.