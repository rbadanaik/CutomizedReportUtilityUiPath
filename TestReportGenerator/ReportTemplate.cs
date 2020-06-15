using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestReportGenerator
{
	class ReportTemplate
	{
		public static String HeaderTemplate =
			"<html>" +
			"<head>" +
			"	<link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css' integrity='sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm' crossorigin='anonymous'></script>" +
			"	 <link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css\" />" +
			"	<script src='https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js'></script>" +
			"	<script src='https://maxcdn.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js'></script>" +
			"	<style> " +
			"		caption{ " +
			"			caption-side:top;" +
			"			display:table-caption;" +
			"			text-align: left; " +
			"		} " +
			"	table td {" +
			"   	text-overflow: ellipsis;" +
			"		overflow: hidden;" +
			"		white-space: no-wrap;" +
			"		max-width : 100px;" +
			"	}" +
			"		.table td,.table th{" +
			"			padding:10px;" +
			"			vertical-align: top;" +
			"			border-top: 1px solid #dee2e6;" +
			"		}" +
			"		.tbl-detailed-report tbody{" +
			"			max-height: 400px;" +
			"			overflow-y: scroll;" +
			"			display: block;" +
			"		}" +
			"		.tbl-detailed-report tr {" +
			"			display: table-row;" +
			"		} " +
			"		td,th{ " +
			"			display: table-cell; " +
			"			width: 2%; " +
			"			word-break: break-word; " +
			"			word-wrap: break-word; " +
			"		} " +
			"		th{" +
			"			text-align:center " +
			"		} " +
			"		td{ " +
			"			padding-bottom:3px; " +
			"			padding-top:3px; " +
			"			adding-left:1px; " +
			"			padding-right:1px; " +
			"		} " +
			"		input{ " +
			"			border: 1px solid #ddd; " +
			"			padding: 5px; " +
			"			margin-left: 10%; " +
			"			margin-top: 0%; " +
			"			float: right; " +
			"			position: relative; " +
			"			top: 1px; " +
			"			border-radius: 20px; " +
			"		} " +
			"		h3 { " +
			"			color: #2E86C1 " +
			"		} " +
			"		body,img { " +
			"			background-color: #FFFFFF " +
			"		} " +
			"		.summary-pass { " +
			"			background-color:#c3e6cb; " +
			"		} " +
			"		.summary-fail { " +
			"			background-color: #f5c6cb; " +
			"		} " +
			/*"		.summary-skip { " +
			"			background-color: white " +
			"		} " +*/
			"		.executionSummaryTotal td{ " +
			"			font-weight:bold; " +
			"		} " +
			"	</style>" +
			"	<script>" +
			"		function searchTest(tableCounter){" +
			"			var input,filter,table,tr,td,ind;" +
			"			input = document.getElementById('searchInput'+tableCounter);" +
			"			filter= input.value.toLowerCase();" +
			"			table=document.getElementById('reportTable'+tableCounter);" +
			"			tr=table.getElementsByTagName('tr');" +
			"			console.log(input.id+','+table.id+','+filter);" +
			"			for(ind=0;ind<tr.length;ind++){" +
			"				td=  tr[ind].getElementsByTagName('td')[1];" +
			"				if(td){" +
			"					if(td.innerHTML.toLowerCase().indexOf(filter)>=0) " +
			"						tr[ind].style.display='';" +
			"					else " +
			"						tr[ind].style.display='none';" +
			"					}" +
			"				 }" +
			"		}" +
			"	</script>" +
			"</head>";

		public static String ReportTitleAndSummaryTableTemplate =
			"<body>" +
					"	<div style='width: 100%;'>" +
					"		<div style='width: 90%;height: 90px;margin:30px'>" +
					"			<h3 style='float: left;'>" +
					"				<u>TEST EXECUTION REPORT</u>" +
					"			</h3>" +
					"			<img style='float:right;' src='https://www.putnam.com/static/img/putnam-logo.svg' width='350px' height='60px;margin-right:17px'/>" +
					"		</div>" +
					"	<div class=' card-cascade narrower' style='width: 55%;margin-top:5%;margin-left:22.5% ;margin-right: 22.5%;margin-bottom: 2%;'> " +
					"			<div class='px-4'> " +
					"				<div class='table-wrapper shadow-lg'> " +
					"					<table class='shadow-lg card table mb-0 tbl-summary' id='summaryTable'>" +
					"						<caption class='card-header'>Test Execution Summary</caption>" +
					"						<thead>" +
					"							<tr>" +
					"								<th class='th-lg'>Workflow</th>" +
					"								<th class='th-lg'>Total</th>" +
					"								<th class='th-lg'>Passed</th>" +
					"								<th class='th-lg'>Failed</th>" +
//					"								<th class='th-lg'>Skipped</th>" +
					"								<th class='th-lg'>Pass %</th>" +
					"							</tr>" +
					"						</thead><tbody>";

		public static String SummaryRowTemplate =
			"<tr class='executionSummary %SUMMARYROWBACKGROUNDCOLOR%' id='%FEATURENAME%'>" +
			"	<td style='margin-top:30px;margin-bottom:2px'>" +
			"		<div id='container'>" +
			"			<a class ='executionSummary' style=\"text-transform: capitalize;\" href='#reportTable%COUNTER%\'>%FEATURENAME%</a>" +
			"		</div>" +
			"	</td>" +
			"	<td align='center' class='totalSummary'>%TOTAL%</td>" +
			"	<td align='center' class='passedSummary'>%PASSED%</td>" +
			"	<td align='center' class='failedSummary'>%FAILED%</td>" +
			// "	<td align='center' class='skippedSummary'>%SKIPPED%</td>" +
			"	<td align='center' class='prcntgPassSummary'>%PASSPERCENTAGE%%</td>" +
			"</tr>	";

		public static String ExecutionSummaryTotalsTemplate =
			"<tr class='executionSummaryTotal' >" +
			"	<td align='center'>Total</td>" +
			"	<td align='center' id='summTotalTotal'>%TOTALTESTS%</td>" +
			"	<td align='center' id='summTotalPass'>%TOTALPASSED%</td>" +
			"	<td align='center' id='summTotalFail'>%TOTALFAILED%</td>" +
			"	<td align='center' id='summTotalFail'>%TOTALSKIPPED%</td>" +
			"	<td align='center' id='summTotalPrcntg'>%TOTALPERCENTAGE%</td>" +
			"</tr>";

		public static String TableClosingTags =
			"				</tbody>" +
			"			</table>" +
			"		</div>" +
			"	</div>" +
			"</div>";

		public static String FeatureTableHeaderTemplate =
			"<div id=\"accordion\">" +
			"<div class='card-cascade narrower' style='width: 80%;margin-top:7%;margin-left:10%;margin-right: 10%;margin-bottom: 2%;'>" +
			"	<div class='px-4'> <div class='table-wrapper shadow-lg'>" +
			"		<table class='card table mb-0 tbl-detailed-report shadow-lg' id='reportTable'> " +
			"			<caption class='card-header'>%FEATURENAME% " +
			"				<a style='font-size:14px' href='#summaryTable'> " +
			"					<sup>Top</sup>" +
			"				</a> " +
			"				<input width='100%' type='text' id='searchInput' placeholder='Search By Status' " +
			"					onkeyup='searchTest()'/> " +
			"			</caption>" +
			"			<thead>" +
			"				<tr>" +
			"					<th>Scenario</th>" +
			"					<th>Status</th>" +
//			"					<th>Error</th>" +
			"					<th>Snapshot Link</th>" +
			"					<th>Time Taken (in Secs)</th>" +
			"				</tr>" +
			"			</thead>" +
			"			<tbody>	";

		public static String ScenarioTemplate =
			"<tr class='markError %ALERTTYPE%'>" +
			"	<td><a style=\"text-transform: capitalize;\" class=\"card-link\" data-toggle=\"collapse\" href=\"#%STEPSTABLE%\">%SCENARIONAME%</a></td>" +
			"	<td align='center'>%SCENARIOSTATUS%</td>" +
			/*"	<td align='center'>" +
			"		<div class='container'>%ALERTERRORMESSAGE%</div>" +
			"	</td>" +*/
			"	<td align='center'>%SNAPSHOTLINK%</td>" +
			"	<td align='center'>%TIMETAKEN%</td>" +
			"</tr>";

		public static String ReportClosingTags =
			"			</div>" +
			"		</div>" +
			"	</body>" +
			"</html>";
	}
}
