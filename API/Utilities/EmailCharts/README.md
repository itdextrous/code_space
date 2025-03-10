# Email Chart Generator

This folder contains the script to generate the donut charts used in email generation.

This generates individual images for each percentage point from 0% through 100%, as well as an image for "N/A" - when no score exists.

## Prerequisites

The computer this is run from must have [GraphicsMagick](http://www.graphicsmagick.org/index.html) installed and added to the PATH (note: this is done by the Windows installer).

## Running

To run, simply run the `GenerateCharts.ps1` PowerShell script. Images will be output to the "output" subfolder.