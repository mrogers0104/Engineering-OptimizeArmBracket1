Utility Structure Arm Connections

*Analysis and Design Procedure by Hock Lim*

This document contains the Sabre Arm Connection analysis and design procedure.  This procedure will be used to analyze and design utility structure arm connections.  The connection includes thru plates on the structure and brackets on the arms.

Mark Rogers

12/15/2017

# Table of Contents
[Arm Connection Analysis](#arm-connection-analysis)

>[Nomenclature](#nomenclature)

>[Arms](#arms)

>[Bolts](#bolts)

>>[Bolt Properties](#properties)

>>[Bolt Thread Length, <i>T<sub>len</sub></i>](#thread-length-tlen)

>>[Bolt Length, <i>B<sub>len</sub></i>](#bolt-length-blen)

>>[Bolt Moment Arm, <i>M<sub>arm</sub></i>](#moment-arm-marm)

>>[Bolt Centerline Loads](#bolt-centerline-loads)

>>[Bolt Group](#bolt-group)

>>[Bolt Stress per bolt, ksi](#bolt-stress-per-bolt)

>>[Bolt Forces](#bolt-forces)

>[Stiffeners](#stiffeners)

>>[Stiffener Width](#width-w)

>>[Stiffener Vertical Spacing, K](#vertical-spacing-k)

>>[Fillet Weld](#fillet-weld)

>[Brackets](#brackets)

>>[Minimum Edge Distance, *e*](#minimum-edge-distance-e)

>>[Minimum Bracket Height, *H*](#minimum-bracket-height-h)

>>[Inside Bracket Face to Center of Bolt, *A*](#inside-bracket-face-to-center-of-bolt-a)

>>[Bracket Opening: Coped or Non-Coped, *L*](#bracket-opening-coped-or-non-coped-l)

>>[Arm Bracket Weld](#arm-bracket-weld)

>>[Bracket Leg Properties](#bracket-leg-properties)

>>[Bracket Leg Bending](#bracket-leg-bending)

>>[Bracket Results](#bracket-results)

>[Thru Plate Length](#thru-plate-length)

>>[Length](#length)

>>[Edge Distance (Clearance)](#edge-distance-clearance)

>[Connection Analysis Summary](#connection-analysis-summary)

>[Warnings](#warnings)

>>[W/t too high!](#wt-too-high)

[Arm Connection Design](#arm-connection-design)

[Appendix A](#appendix-a)

>[Bolt Properties](#bolt-properties)

>[Bolt Specs](#bolt-specs)

>[Arm to Bracket Weld](#arm-to-bracket-weld)

[Appendix B](#appendix-b)




# Arm Connection Analysis

<!-- <a name="_toc501109890"></a>B

h<sub>s</sub>

h<sub>s</sub>
## ![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.005.png) -->
<!-- ![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.006.png)
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.007.png)
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.008.png)
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.009.png)
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.010.png)
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.011.png)
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.012.png)
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.013.png)
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.014.png)
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.015.png) -->

<table>
  <tr>
  <td>Bolts</td>
    <td><img src="Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.009.png" align="right">
    </td>
  </tr>
  <tr>
  <td>Bolts</td>
    <td><img src="Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.012.png" align="right">
    </td>
  </tr>
  <tr>
  <td>Bracket</td>
    <td><img src="Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.013.png" align="right">
    </td>
    <td><img src="Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.014.png" align="right">
    </td>
  </tr>
  <tr>
  <td>Stiffener</td>
    <td><img src="Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.010.png" align="right">
    </td>
    <td><img src="Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.011.png" align="right">
    </td>
  </tr>
  <tr>
  <td>Thru Plate </td>
    <td><img src="Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.015.png" align="right">
    </td>
  </tr>
</table>

## **Nomenclature**


**Arm**
|**Value**|**Description**|
| :-: | :-: |
|<i>D<sub>ff</sub></i>|Diameter Flat-Flat, in.|
|<i>D<sub>pp</sub></i>|Diameter Point-Point, in.|
|<i>D<sub>H</sub></i>|<i>hex<sub>n</sub></i> Height, in.|
|<i>T<sub>a</sub></i>|Wall thickness, in.|
|**Bolts**|
|**Value**|**Description**|
|*dia*|Diameter, in.|
|<i>qty<sub>b</sub></i>|Total bolt quantity|
|*c*|Center Space, in.|
|*s*|Spacing, in.|
|*e*|Side Edge, in.|
|*b*|Top & Bottom Edge, in.|
|<i>F<sub>y</sub></i>|Yield Strength, ksi|
|<i>F<sub>u</sub></i>|Ultimate Strength, ksi|
|<i>A<sub>g</sub></i>|Gross Area, in<sup>2.</sup>|
|<i>A<sub>t</sub></i>|Tensile Area, in<sup>2.</sup>|
|<i>A<sub>B</sub></i>|Total Area (<i>A<sub>g</sub> * qty<sub>b</sub></i>), in<sup>2.</sup>|
|<i>T<sub>len</sub></i>|Thread Length, in.|
|<i>B<sub>len</sub></i>|Bolt length, in.|
|**Bracket**|
|**Value**|**Description**|
|<i>F<sub>yb</sub></i>|Yield Strength, ksi|
|<i>F<sub>ub</sub></i>|Ultimate Strength, ksi|
|<i>T<sub>b</sub></i>|Thickness, in.|
|*L*|Opening, in.|
|*H*|Height, in.|
|*A*|Inside Face to Bolt, in.|
|*B*|Leg Length (<i>A + e + T<sub>b</sub></i>), in.|
|*R*|Bend Radius, in. (default=3<i>T<sub>b</sub></i>)|
|**Stiffener**|
|**Value**|**Description**|
|<i>T<sub>s</sub></i>|Thickness, in.|
|*K*|Spacing, in.|
|<i>qty<sub>s</sub></i>|Quantity|
|<i>h<sub>s</sub></i>|Min. Width, in(default=2.5”)|
|**Thru Plate**|
|**Value**|**Description**|
|<i>F<sub>yt</sub></i>|Yield Strength, ksi|
|<i>F<sub>ut</sub></i>|Ultimate Strength, ksi|
|<i>T<sub>t</sub></i>|Thickness, in.|
|*W*|Opening, in.|
|*F*|Flat to Bolt, in.|
|*H*|Height, in.|
|<i>OD<sub>p</sub></i>|Pole Outside Dia, in.|
|*E*|Edge Distance, in.|

NOTE: bracket height, *H*, and thru plate height, *H*, are equal.

The Thru Plate Opening, *W*, is the base of a design for the connection.  Since the difference between *W* and *L* is small (1/8”), engineering will use the rounded value, that is, the Thru Plate Opening, *W.*
## Arms

|**Arm Shape Properties**|
| :-: |
|**Value**|**Description**|
|*n*|Number of Sides|
|<i>q<sub>f</sub></i>|Flat angle, rad; ![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.016.png)|
|<i>f<sub>w</sub></i>|Flat width, in;![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.017.png)|
|<i>D<sub>ff</sub></i>|Arm diameter (flat-flat), in.|
|<i>D<sub>pp</sub></i>|Arm OD (point-point), in.; ![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.018.png)|
|<i>D<sub>W</sub></i>|Arm width, in.|
|<i>D<sub>H</sub></i>|Arm height, in.|
|*f*|Shape height factor; default = 1|
|*re*|remainder|

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.019.png)

Arm shape type is determined as follows:

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.020.png)
#### Orientation: Flat-to-Flat
For *rem* = 0, 

<i>D<sub>W</sub> = D<sub>ff</sub></i>.

<i>D<sub>H</sub> = f * D<sub>ff</sub></i>.

For *rem* ≠ 0, 

<i>D<sub>W</sub> = D<sub>ff</sub></i>.

<i>D<sub>H</sub> = f * D<sub>pp</sub></i>.
#### Orientation: Point-to-Point
For *rem* = 0, 

<i>D<sub>W</sub> = D<sub>pp</sub></i>.

<i>D<sub>H</sub> = D<sub>pp</sub></i>.

For *rem* ≠ 0, 

<i>D<sub>W</sub> = f * D<sub>pp</sub></i>.

<i>D<sub>H</sub> = D<sub>ff</sub></i>.

#### <a name="_ref501109741"></a>Irregular Arm Shape
Irregular arm shapes occur when *rem* ≠ 0 and the polygon is elongated along the flat, for example, *hex1* through *hex6*.

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.021.png)Irregular arms will always be oriented Flat-to-Flat.

|**Shape**|
| :-: |
|**Label**|**Description**|
|*hex1*|Hex 2.0 to 1|
|*hex2*|Hex 1.8to 1|
|*hex3*|Hex 1.6 to 1|
|*hex4*|Hex 1.5 to 1|
|*hex5*|Hex 1.4 to 1|
|*hex6*|Hex 1.25 to 1|

The shape description shows the shape factor, *f*.

Description: Hex *f* to 1

For example, hex1 width, <i>D<sub>ff</sub></i> = 6.0, then <i>f</i> = 2.0.  Therefore, <i>D<sub>H</sub> =</i> <i>f * D<sub>ff</sub></i> = 2.0 * 6.0 = 12”
## Bolts
### Properties
F<sub>y</sub> = see Appendix A:  Bolt Specs either Table 3 or Table 4, given bolt diameter and bolt specification.

<i>Shear Capacity</i> (threads excluded), <i>F<sub>Bv</sub></i> = 0.45 * F<sub>y</sub>

<i>Shear Capacity</i> (threads included), <i>F<sub>Bv</sub></i> = 0.35 * F<sub>y</sub>
#### Bolt Spacing
Standard Bolt Spacing, *s* = spacing given bolt diameter from Table 1 in Bolt Properties

Center Spacing, *c*:

If <i>qty<sub>b</sub></i> % 4 = 0 then <i>c<sub>0</sub></i> = s/2 else <i>c<sub>0</sub></i> = s

<i>c<sub>i</sub></i> = <i>c<sub>0</sub></i> + <i>i * s</i>

where *i* = 0 … n

**NOTE**: when ![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.022.png) is an odd number, Center spacing, *c* = 0

**For design,** 

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.023.png)

Where: 

`	`*c* = center spacing, in.

`	`*b* = top edge distance, in.; usually = *s/2*

`	`*j* = number of bolts in half the bracket leg, ![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.024.png)

`	`*s* = bolt spacing, in.
### Thread Length, <i>T<sub>len</sub></i> 
For bolt diameter, <i>dia</i> > 1.50”, <i>T<sub>len</sub></i> = <i>dia</i> + 1”

Otherwise, use the values shown in Table 1 in Bolt Properties.
### Bolt Length, <i>B<sub>len</sub></i>
For bolt diameter, *dia* > 1.50” then

`	`<i>B<sub>len</sub></i> = <i>T<sub>b</sub></i> + <i>T<sub>t</sub></i> + 0.75”

Otherwise

`	`<i>B<sub>len</sub></i> = <i>T<sub>b</sub></i> + <i>T<sub>t</sub></i> + <i>T<sub>len</sub></i>
### Moment Arm, <i>M<sub>arm</sub></i>
The bolt moment arm depends on the arms shape and orientation.
#### Hexagonal Arm
##### *Orientation: Flat-to-Flat*
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.025.png)![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.026.png)


##### *Orientation: Point-to-Point*
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.027.png)
#### All Other Arm Shapes
Where number of sides of the polygon is divisible by 4.
##### ![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.028.png)*Orientation: Flat-to-Flat*


![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.027.png)



##### *Orientation: Point-to-Point*
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.029.png)![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.026.png)


### Bolt Centerline Loads
<i>f<sub>T</sub></i>

<i>M<sub>L</sub></i>

<i>M<sub>T</sub></i>

<i>f<sub>v</sub></i>

<i>M<sub>v</sub></i>

<i>f<sub>L</sub></i>
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.030.png)

|**PLS Load**|
| :-: |
|**Value**|**Description**|
|<i>f<sub>v</sub></i>|Vertical Force, kips|
|<i>f<sub>T</sub></i>|Translational Force, kips|
|<i>f<sub>L</sub></i>|Longitudinal Force, kips|
|<i>M<sub>v</sub></i>|Vertical Moment, ft-kips|
|<i>M<sub>T</sub></i>|Translational Moment, ft-kips|
|<i>M<sub>L</sub></i>|Longitudinal Moment, ft-kips|
|**Load @ Centerline of Bolts**|
|**Value**|**Description**|
|<i>f<sub>vb</sub></i>|Vertical Force, kips|
|<i>f<sub>Tb</sub></i>|Translational Force, kips|
|<i>f<sub>Lb</sub></i>|Longitudinal Force, kips|
|<i>M<sub>vb</sub></i>|Vertical Moment, ft-kips|
|<i>M<sub>Tb</sub></i>|Translational Moment, ft-kips|
|<i>M<sub>Lb</sub></i>|Longitudinal Moment, ft-kips|

Vertical Moment, <i>M<sub>vb</sub></i>

`	`From Pole Face: ![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.031.png)

`	`From Bracket Face: ![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.032.png)

Longitudinal Moment, <i>M<sub>Tb</sub></i>

`	`From Pole Face: ![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.033.png)

`	`From Bracket Face: ![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.034.png)
#### PLS Cross Reference

|**PLS Cross Reference**|
| :-: |
|**Value**|**PLS Value**|
|<i>f<sub>T</sub></i>|Axial Force|
|<i>f<sub>v</sub></i>|Vertical Shear|
|<i>f<sub>L</sub></i>|Horizontal Shear|
|<i>M<sub>v</sub></i>|Vertical Moment|
|<i>M<sub>T</sub></i>|Torsion|
|<i>M<sub>L</sub></i>|Horizontal Moment|

### Bolt Group
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.035.png) 
### Bolt Stress per bolt
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.036.png)
### Bolt Forces
Bolt forces are calculated for each bolt on ½ the bracket leg.  Bolts are numbered 1 … i.  For example, if there are a total of 8 bolts, there will be 4 per leg and *j* = 2 per ½ of the leg.  The values below would be calculated for *i* = 1 to 2.

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.037.png)

Where:

|*j*|Number of bolts considered|
| :-: | :- |
|*i*|Bolt number under consideration|
|<i>T<sub>arm</sub></i>|Distance from bolt to arm butt, in.|
|<i>d<sub>i</sub></i>|Distance from bracket centerline to bolt|
|<i>d<sub>i</sub><sup>2</sup></i>|Square of the distance, *d*|
|<i>S<sub>xi</sub></i>|Shear – X stress, ksi|
|<i>S<sub>zi</sub></i>|Shear – Z stress, ksi|
|<i>S<sub>xzi</sub></i>|Combined stress, ksi|
|<i>f<sub>vi</sub></i>|Shear force, kips|
|*M*|Moment, ft-kips|

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.038.png)		Maximum Bolt Shear stress, ksi

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.039.png)		Maximum Bolt Shear, kips

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.040.png)		Bolt Shear Interaction ratio

## Stiffeners
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.041.png)

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.042.png)
### Width, *W*
This is an interpolated method using the Standard Bracket Table dated 11/07/17.

If <i>W</i> ≤ 15” and <i>T<sub>a</sub></i> < 0.375” then no stiffener required.

If <i>W</i> ≤ 28” and <i>T<sub>a</sub></i> < 0.375” then

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.043.png)

Otherwise

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.044.png)

Round <i>h<sub>s</sub></i> up to nearest ¼”.
### Vertical Spacing, *K*
The vertical spacing of the stiffeners is computed as follows:

For a regular polygon and <i>qty<sub>s</sub></i> ≥ 2

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.045.png)

For a <i>hex1</i> thru <i>hex6</i> and <i>qty<sub>s</sub></i> ≥ 2 (See Irregular Arm Shape above)

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.046.png)

Note: this is the Fort Worth Tower method.
### Fillet Weld
Fillet weld size, <i>f<sub>sw</sub></i>, is shown in the table below.

||<i>f<sub>sw</sub></i>|
| :-: | :-: |
|0\.25 < <i>T<sub>b</sub></i> ≤ 0.50|0\.1875”|
|0\.50 < <i>T<sub>b</sub></i> ≤ 0.75|0\.2500”|
|<i>T<sub>b</sub></i> > 0.75|0\.5000”|

## Brackets
### Minimum Edge Distance, *e* 
<i>e<sub>min1</sub></i> = Min. edge distance given bolt diameter from Table 1 in Bolt Properties

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.047.png)

<i>e<sub>min</sub></i> = max(<i>e<sub>min1</sub>, e<sub>min2</sub>, e<sub>min3</sub>,</i>)</i> 
### Minimum Bracket Height, *H*
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.048.png)

See Arm Properties for a definition of <i>D<sub>W</sub>.</i>
### Inside Bracket Face to Center of Bolt, *A*
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.049.png)
### Bracket Opening: Coped or Non-Coped, *L*
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.050.png)

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.051.png)


### Arm Bracket Weld
Threshold, <i>w<sub>th</sub></i> = Threshold given arm wall thickness, <i>T<sub>a</sub></i>, from Table 4 in Arm to Bracket Weld

Fillet Weld, <i>f<sub>bw</sub></i>:

CJP specified – CJP given arm wall thickness, <i>T<sub>a</sub></i>, from Table 4 in Arm to Bracket Weld

Otherwise, 

<i>if D<sub>ff</sub> < w<sub>th</sub> then</i> PJP-1 given arm wall thickness, <i>T<sub>a</sub></i>, from Table 4 in Arm to Bracket Weld

*else*  

PJP-2 given arm wall thickness, <i>T<sub>a</sub></i>, from Table 4 in Arm to Bracket Weld
### Bracket Leg Properties
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.052.png)
### Bracket Leg Bending
Moment arm, <i>M<sub>armBi</sub></i> and Moment, <i>M<sub>bkti</sub></i> for bracket bolts:

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.053.png)

The bending stress in the bracket leg is

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.054.png)		Bracket leg bending stress
### Bracket Results

#### Bracket Thickness
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.055.png)		Minimum required bracket thickness, in.

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.056.png)			Bracket thickness capacity, ksi

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.057.png)			Ratio of capacity to allowable for bracket thickness. 

(< 1.0 is overstressed)
#### Bracket Stress
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.058.png)	Stiffener bending stress, ksi

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.054.png)		Bracket leg bending stress, ksi

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.059.png)	Maximum bracket stress, ksi

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.060.png)			Interaction ratio for bracket stress.
#### Shear Rupture
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.061.png)
#### Bearing
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.062.png)![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.063.png)
## Thru Plate Length
### Length
### Edge Distance (Clearance)
![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.064.png)

Where *m* ≤ 1.50”
## Connection Analysis Summary
- Thru Plate: 		(2) Plate <i>T<sub>t</sub></i> x <i>H</i> x <i>L<sub>t</sub></i>
- Bracket:		(1) Plate <i>T<sub>b</sub></i> x <i>H</i> x ?
- Stiffener:		(2) Plate <i>T<sub>s</sub></i> x <i>h<sub>s</sub></i> x <i>H</i>
- Weld:			Fillet <i>f<sub>sw</sub></i>”
- Arm to Bracket Weld:	CJP <i>f<sub>bw</sub></i>” Reinforced Fillet weld with backup bar
  or			PJP <i>f<sub>bw</sub></i>” Fillet Weld

## Warnings
### W/t too high!
*Stiffener q*ty<sub>s</sub> = 0 then *Limit* = 24

*Stiffener q*ty<sub>s</sub> > 0 then *Limit* = 30

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.065.png)



# Arm Connection Design
The following procedure will be used to design an arm connection.

1. Start with arm diameter, <i>D<sub>ff</sub></i> and arm wall thickness, <i>T<sub>a</sub></i>.
   1. Compute the Bracket Opening, W.  See Bracket Opening: Coped or Non-Coped above.
   1. Compute the Bracket Height, H.  See Minimum Bracket Height above.
   1. Estimate the quantity of bolts required.
      1. Iterate through the bolt diameters shown in Bolt Properties (Table 1) and the associated Bolt Specs for A325 and A354-BC bolts (Table 2 and Table 3) using the following equation and round up to nearest even number.  It is assumed that threads are excluded from the shear plane.
         ![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.066.png)
         NOTE: each bolt will have a different <i>A<sub>g</sub></i> and <i>F<sub>y</sub></i> depending on bolt grade.
         1. Will the <i>qty<sub>b</sub></i> fit on the bracket?
            ![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.067.png)
      1. Use bolt spec and diameter where bolt spacing requirement is met.
      1. Non-standard bolt diameter: 0.875”, 1.125”, and 1.375”
   1. Compute bracket thickness requirement.  This will be an iterative process.
      where minimum (starting) thickness:
      ![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.068.png)


# Appendix A
## Bolt Properties
**Table 1****

|<p>**Bolt Diameter**</p><p>(in)</p>|<p>**Min. Edge, *e***</p><p>(in)</p>|<p></p><p>**Spacing, *s***</p><p>(in)</p>|<p>**Gross Area**</p><p>(in<sup>2</sup>)</p>|<p>**Tensile Area**</p><p>(in<sup>2</sup>)</p>|<p></p><p>**Thread**</p>|<p>**Nut <br>Pt-Pt**</p><p>(in)</p>|<p>**Thread Length**</p><p>(in)</p>|<p>**Max. PL Thk**</p><p>(in)</p>|<p></p><p>**F**</p><p>(in)</p>|
| :-: | :-: | :-: | :-: | :-: | :-: | :-: | :-: | :-: | :-: |
|<a name="range!b14:k27"></a><a name="range!b14:b27"></a>**5/8**|1\.25|2|0\.307|0\.226|11|1\.227|1\.25|15/16|3|
|**3/4**|1\.5|3|0\.442|0\.334|10|1\.443|1\.375|1  1/8|3|
|**7/8**|1\.5|3|0\.601|0\.462|9|1\.66|1\.5|1  1/16|3|
|**1**|1\.5|3|0\.785|0\.606|8|1\.876|1\.75|1|3|
|**1  1/8**|1\.5|3|0\.994|0\.763|7|2\.093|2|15/16|3|
|**1  1/4**|1\.75|3\.5|1\.227|0\.969|7|2\.309|2|1  1/8|3|
|**1  3/8**|2|3\.75|1\.485|1\.16|6|2\.526|2\.25|1  5/16|3|
|**1  1/2**|2|4|1\.767|1\.41|6|2\.742|2\.25|1  1/4|3|
|**1  3/4**|2\.5|4\.75|2\.405|1\.9|5|3\.175|99|1  5/8|4|
|**2**|2\.75|5\.5|3\.142|2\.5|4\.5|3\.608|99|1  3/4|4|
|**2  1/4**|3|6|3\.976|3\.25|4\.5|4\.041|99|1  7/8|4|
|**2  1/2**|3\.5|6\.75|4\.909|4|4|4\.474|99|2  1/4|5|
|**2  3/4**|3\.75|7\.5|5\.94|4\.93|4|4\.907|99|2  3/8|5|
|**3**|4|8|7\.069|5\.97|4|5\.34|99|2  1/2|5|

**NOTE:** If *e + ¾” > F*, then *F = e + ¾"*

If Bolt Diameter > 1.50” then Thread Length = Bolt Diameter + 1.00”

*s* = Bolt Diameter \* (2 + 2/3)   round up to nearest ¼”
## Bolt Specs
**Table 2****

|**Spec**|**Fy** (ksi)|**Fu** (ksi)|
| :-: | :-: | :-: |
||<b>*<sub>1</sub></b>|<b>*<sub>2</sub></b>|<b>*<sub>3</sub></b>|<b>*<sub>1</sub></b>|<b>*<sub>2</sub></b>|<b>*<sub>3</sub></b>|
|<a name="range!q3:q8"></a>**A325**|92|81|n/a|120|105|n/a|
|**A449**|92|81|58|120|105|90|

<b>*<sub>1</sub></b> for bolt dia  ≤ 1”

<b>*<sub>2</sub></b> for bolt dia > 1” and ≤ 1.5”

<b>*<sub>3</sub></b> for bolt dia > 1.5”

<a name="_ref492640408"></a>**Table 3****

|**Spec**|**Fy** (ksi)|**Fu** (ksi)|
| :-: | :-: | :-: |
||<b>*<sub>1</sub></b>|<b>*<sub>2</sub></b>||<b>*<sub>1</sub></b>|<b>*<sub>2</sub></b>||
|**A193-B7**|105|95|n/a|125|115|n/a|
|**A354-BC**|109|94|n/a|125|115|n/a|
|**A354-BD**|130|115|n/a|150|140|n/a|
|**A490**|130||n/a|150|n/a|n/a|

<b>*<sub>1</sub></b> for bolt dia  ≤ 2.5”

<b>*<sub>2</sub></b> for bolt dia > 2.5”

## <a name="_ref492987267"></a><a name="_toc501109924"></a>Arm to Bracket Weld
<a name="_ref492640419"></a>**Table 4****

|||**PJP w/ Fillet (80%)**|**CJP**|
| :- | :-: | :-: | :-: |
|<p><a name="range!q13:u19"></a>**Arm Wall Thick**</p><p>(in)</p>|<p>**Threshold**</p><p>(in)</p>|<p>**1**</p><p>(in)</p>|<p>**2**</p><p>(in)</p>|<p></p><p>(in) </p>|
|**0.1875**|17|0\.375|0\.3125|0\.1875|
|**0.25**|25|0\.4375|0\.375|0\.25|
|**0.3125**|30|0\.5625|0\.5|0\.3125|
|**0.375**|34|0\.6875|0\.625|0\.3125|
|**0.4375**|43|0\.75|0\.6875|0\.375|
|**0.5**|47|0\.875|0\.8125|0\.4375|



# Appendix B
Backend Cross Reference

|**Value**|**Backend Name**|
| :-: | :-: |
|*F*|PoleFactToBoltCenterLine|
|<i>T<sub>b</sub> + A</i>|BracketSideWidth|



BracketSideWidth defined in ????  (Two ways to define this!?!?)

![](Documentation/ArmAndConnectionDesignProcedure%20-%20mdVer/Aspose.Words.abdc75ce-347c-4adb-8ed2-71ed4484501e.069.png)