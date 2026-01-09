# App Icons Directory

This directory contains the image files for application icons.

## Required Icons

Place PNG image files (recommended size: 24x24 to 48x48 pixels) with the following names:

### Apps Icons:
- `SepalWRFM.png` - SEPAL WRFM icon
- `BFS.png` - BFS icon
- `EPS.png` - EPS icon
- `SMBS.png` - SMBS icon
- `DIAP.png` - DIAP icon
- `REINEUR.png` - REINEUR icon
- `ESTURDI.png` - ESTURDI icon
- `DOKU.png` - DOKU icon
- `SharePoint.png` - SharePoint icon

### Cross Platform Apps Icons:
- `Bookings.png` - Bookings icon
- `Copilot.png` - Copilot icon
- `OrgExplorer.png` - Org Explorer icon (note: no space in filename)
- `Sales.png` - Sales icon

### Work Apps Icons:
- `Project.png` - Project icon
- `Visio.png` - Visio icon
- `Forms.png` - Forms icon

### Default Icon:
- `Default.png` - Fallback icon (used when an icon is not found)

## Image Specifications

- **Format**: PNG (with transparency support)
- **Recommended Size**: 24x24 to 48x48 pixels
- **Background**: Transparent (preferred)
- **Color**: Full color or monochrome (will be displayed on colored backgrounds)

## Adding New Icons

1. Add your PNG image file to this directory
2. Update `Resources/IconResources.xaml` to add a new mapping:
   ```xml
   <x:String x:Key="Icon_YourIconName">pack://application:,,,/SepalWRFM;component/Images/Icons/YourIconName.png</x:String>
   ```
3. Use the icon name in your ViewModel (e.g., `Icon = "YourIconName"`)
