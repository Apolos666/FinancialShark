import React from "react";
import Table from "../../Components/Table/Table";
import RatioList from "../../Components/RatioList/RatioList";
import {CompanyKeyMetrics} from "../../company";
import {testIncomeStatementData} from "../../Components/Table/testData";

interface Props {
};

const tableConfig = [
    {
        label: "Market Cap",
        render: (company: CompanyKeyMetrics) =>
            company.marketCapTTM,
        subTitle: "Total value of all a company's shares of stock",
    },
]

const DesignPage: React.FC<Props> = ({}: Props): JSX.Element => {
    return (
        <div>
            <h1>FinShark Design Page</h1>
            <h2>
                This is FinShark's design page. This is where we well house various design elements and components that we use throughout the application.
            </h2>
            <RatioList data={testIncomeStatementData} config={tableConfig} />
            <Table config={tableConfig} data={testIncomeStatementData}/>
        </div>
    )
}

export default DesignPage;