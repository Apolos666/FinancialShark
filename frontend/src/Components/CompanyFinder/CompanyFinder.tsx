import React, {useEffect, useState} from "react";
import {CompanyCompData} from "../../company";
import {getCompData} from "../../api";
import CompanyFinderItem from "./CompanyFinderItem/CompanyFinderItem";

interface Props {
    ticker: string;
};

const CompanyFinder: React.FC<Props> = ({ticker}: Props): JSX.Element => {
    const [companyData, setCompanyData] = useState<CompanyCompData>();

    useEffect(() => {
        const getCompanyFinderAsync = async () => {
            const result = await getCompData(ticker);
            setCompanyData(result?.data[0]);
        }

        getCompanyFinderAsync();
    }, [ticker]);

    return (
        <div className="inline-flex rounded-md shadow-sm m-4">
            {companyData?.peersList.map((ticker) => {
                return <CompanyFinderItem ticker={ticker}/>
                }
            )}
        </div>
    )
}

export default CompanyFinder;