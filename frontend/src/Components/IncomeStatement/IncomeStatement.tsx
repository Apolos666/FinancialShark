import React, {useEffect, useState} from "react";
import {CompanyIncomeStatement} from "../../company";
import {useOutletContext} from "react-router-dom";
import {getIncomeStatement} from "../../api";
import Table from "../Table/Table";
import Spinner from "../Spinner/Spinner";
import {formatLargeMonetaryNumber, formatRatio} from "../../Helpers/NumberFormatting";

interface Props {
};

const configs = [
    {
        label: "Date",
        render: (company: CompanyIncomeStatement) => company.date,
    },
    {
        label: "Revenue",
        render: (company: CompanyIncomeStatement) =>
            formatLargeMonetaryNumber(company.revenue),
    },
    {
        label: "Cost Of Revenue",
        render: (company: CompanyIncomeStatement) =>
            formatLargeMonetaryNumber(company.costOfRevenue),
    },
    {
        label: "Depreciation",
        render: (company: CompanyIncomeStatement) =>
            formatLargeMonetaryNumber(company.depreciationAndAmortization),
    },
    {
        label: "Operating Income",
        render: (company: CompanyIncomeStatement) =>
            formatLargeMonetaryNumber(company.operatingIncome),
    },
    {
        label: "Income Before Taxes",
        render: (company: CompanyIncomeStatement) =>
            formatLargeMonetaryNumber(company.incomeBeforeTax),
    },
    {
        label: "Net Income",
        render: (company: CompanyIncomeStatement) =>
            formatLargeMonetaryNumber(company.netIncome),
    },
    {
        label: "Net Income Ratio",
        render: (company: CompanyIncomeStatement) =>
            formatRatio(company.netIncomeRatio),
    },
    {
        label: "Earnings Per Share",
        render: (company: CompanyIncomeStatement) => formatRatio(company.eps),
    },
    {
        label: "Earnings Per Diluted",
        render: (company: CompanyIncomeStatement) =>
            formatRatio(company.epsdiluted),
    },
    {
        label: "Gross Profit Ratio",
        render: (company: CompanyIncomeStatement) =>
            formatRatio(company.grossProfitRatio),
    },
    {
        label: "Opearting Income Ratio",
        render: (company: CompanyIncomeStatement) =>
            formatRatio(company.operatingIncomeRatio),
    },
    {
        label: "Income Before Taxes Ratio",
        render: (company: CompanyIncomeStatement) =>
            formatRatio(company.incomeBeforeTaxRatio),
    },
];

const IncomeStatement: React.FC<Props> = ({}: Props): JSX.Element => {
    const ticket = useOutletContext<string>();
    const [incomeStatementData, setIncomeStatementData] = useState<CompanyIncomeStatement[]>();

    useEffect(() => {
        const getIncomeStatementAsync = async () => {
            const value = await getIncomeStatement(ticket!);
            setIncomeStatementData(value!.data);
        }

        getIncomeStatementAsync();
    }, []);

    return (
        <div>
            {incomeStatementData ? (
                <Table config={configs} data={incomeStatementData}/>
            ) : (
                <Spinner />
            )}
        </div>
    )
}

export default IncomeStatement;