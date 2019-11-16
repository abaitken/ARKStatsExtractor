[**Download** the latest release](https://github.com/cadon/ARKStatsExtractor/releases/latest).

**Which version** to take?

* Use the setup-ArkSmartBreeding-(version).**exe** for an installation in the system's programs folder. Suitable for single user installations.
* Extract the ARK.Smart.Breeding_(version).**zip** in an arbitrary folder to use it as a portable or shared installation.

**Discord**: [https://discord.gg/qCYYbQK](https://discord.gg/qCYYbQK)

# ARK Smart Breeding
For the game ARK Survival Evolved. Extracts possible levelups of creatures to get the values for breeding. Save your creatures in a library, 
sort and compare their stats, view their pedigree, use the breeding-plan to get the best possible creatures and keep track of the growing babies with timers.

## Known issues
### Ark Bugs 
#### Wrong Stat Values
The following species may show wrong stat-values ingame that prevents a correct extraction. Mostly the stat health is affected, sometimes weight, stamina or food. Currently there's no known workaround for this bug in ARK.
* Titanoboa
* Aberrant Titanoboa
* Pegomastax
* Procoptodon
* Troodon
* Gacha
* Pulmonoscorpius
* Aberrant Pulmonoscorpius
* Electrophorus
* Aberrant Electrophorus
* Desert Titan
* Desert Titan Flock
* Ice Titan

#### Imprinting Bug
* Creatures sometimes forget to increase their values properly after imprinting
* Force creature to recalculate stats by:
  * Upload/Download from Tek Transmitter, Supply Drop, or Obelisk
  * Use a Cryopod
  * Leave render range for a couple of minutes and return
  
### ASB Issues 
#### Capture Ark Window (OCR)
* Rather inaccurate
* Limited details about creatures
* Limited supported resolutions
* **Use [Creature Import](https://github.com/cadon/ARKStatsExtractor/wiki/Importing-Creatures#ark-exports) instead**

## Manual
See the wiki on more info, e.g. [Manual](https://github.com/cadon/ARKStatsExtractor/wiki/Manual) with links to external resources like **guides and videos**, 
or [Extraction issues](https://github.com/cadon/ARKStatsExtractor/wiki/Extraction-issues) if something does not work.

#### Library
![Library](img/library.png)

## Screenshots
##### Extractor
![Screenshot](img/extractor.png)
##### Breeding Plan
![Screenshot](img/breedingplan.png)
##### Pedigree
![Screenshot](img/pedigree.png)
##### Taming Infos
![Screenshot](img/taming.png)
##### Raising List
![Screenshot](img/raising.png)

## Download
Download the [latest release here](https://github.com/cadon/ARKStatsExtractor/releases/latest).

* The file values.json contains all the stats, default-multipliers and other values (taming, breeding), it can be edited and updated if necessary.
* The image-files for the colored-creature-views have to be downloaded separately: [Creature-Images](https://github.com/cadon/ARKStatsExtractor/raw/master/images.zip). 
Extract the folder "img" in the application's folder to get better visuals of the creature's colors. 
Currently only a few creatures are included. You don't need to redownload this file if you already have the creature-images.

## Patchnotes
For a full list see [Releases](https://github.com/cadon/ARKStatsExtractor/releases).