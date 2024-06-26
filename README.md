# War Thunder Data Scraper

This tool aims to be able to automatically scrape and save data directly from the [War Thunder Wiki](wiki.warthunder.com) and the [War Thunder Replays](https://warthunder.com/en/tournament/replay/)

## Preface

This code isn't necessarily designed for everyone and their mother to create a comprehensive database of everything they could possibly want off of the War Thunder wiki, but I am putting it out there for anyone who wishes to create something similar or just to know how I gathered the data that I am planning on releasing when I am finished with another future project.

## Reason

There currently is no comprehensive data sheet with all of the relevant and useful information on the wiki, and non-aggregated data from replays, especially one that can be easily used for statistical analysis. To address this problem, I am going ahead to make a tool for the purposes of scraping as much of the statistics as possible. I'm going to be starting with the ground trees (with a focus on realistic battles) first due to simplicity and cohesiveness, but I will probably add Air, and possibly Naval later.

## Installation

### Requirements

Uses [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (7 should also work if you change it in the `.csproj` file), and the nuget package [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack) (although it should download automatically during the build process).

### Git Clone

Clone the repository with `git clone https://github.com/DoctorAmerica/wtdatascraper`

### Installing Packages

Install the packages either in Visual Studio's built-in nuget package installer or with the `dotnet` commands provided in the links to the relevant packages in [Requirements](###Requirements) (make sure to be in the cloned repo when running `dotnet` commands)

### Building/Running

Build with `dotnet build -c Release` and find the relevant executable and libraries under `wtdatascraper>bin>Release`
or simply run with `dotnet run --project wtdatascraper -c Release` (though I recommend `dotnet build -c Release && dotnet run --project wtdatascraper -c Release` to do both at the same time). It takes about half an hour to an hour to run, though this might be looked into in the future.
