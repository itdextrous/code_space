function Draw-WfChart {
    param (
        [Parameter(Mandatory)]
        [string]$Name,
        [Parameter(Mandatory)]
        [int]$Percentage,
        [Parameter(Mandatory)]
        [string]$Color,
        [Parameter(Mandatory)]
        [string]$Background,
        [int]$Size = 250,
        [string]$Text = $null
    )
    
    # Width of ring is set as 12% of total chart Size
    $width = 0.12 * $Size

    # We remove half of the ring width from the radius, as the stroke is centered on the ring
    $radius = $Size / 2 - $width / 2

    # The center of the page
    $center = $Size / 2

    # 0 degrees is to the right, so -90 is straight up
    $startAngle = -90;
    $endAngle = $Percentage / 100 * 360 + $startAngle;

    # This is in pts - experimentally calculated
    $fontSize = $Size * 0.25

    if ([string]::IsNullOrEmpty($Text)) { $Text = "${percentage}%" }

    gm.exe convert -size "${Size}x${Size}" xc:none `
           -font "fonts/Montserrat-Bold.ttf" `
           -pointsize $fontSize `
           -gravity center `
           -fill transparent `
           -strokewidth $width `
           -stroke $Background `
           -draw "ellipse $center,$center $radius,$radius 0,360" `
           -stroke $Color `
           -draw "ellipse $center,$center $radius,$radius -90,$endAngle" `
           -fill black `
           -stroke none `
           -draw "text 0,0 '$Text'" `
           $Name
}

function Draw-WfCharts {
    param (
        [Parameter(Mandatory)]
        [string]$NamePrefix,
        [Parameter(Mandatory)]
        [string]$Color,
        [Parameter(Mandatory)]
        [string]$Background,
        [int]$Size = 250
    )

    for ($i = 0; $i -le 100; $i++) {
        Draw-WfChart -Name "${NamePrefix}${i}.png" -Percentage $i -Color $Color -Background $Background -Size $Size
    }
    Draw-WfChart -Name "${NamePrefix}na.png" -Percentage 0 -Color $Color -Background $Background -Size $Size -Text "N/A"
}

if (-Not (Test-Path -Path "output")) { New-Item -Name "output" -ItemType directory }
Draw-WfCharts -NamePrefix "output/team_" -Color `#40B4E2 -Background `#B3E1F3
Draw-WfCharts -NamePrefix "output/goal_" -Color `#03AF4B -Background `#CDEFDB
Draw-WfCharts -NamePrefix "output/number_" -Color `#E61A4F -Background `#FAD1DC
Draw-WfCharts -NamePrefix "output/action_" -Color `#F58220 -Background `#FBCDA6
Draw-WfCharts -NamePrefix "output/discipline_" -Color `#1C439B -Background `#A4B4D6
Draw-WfCharts -NamePrefix "output/team_large_" -Color `#40B4E2 -Background `#B3E1F3 -Size 500