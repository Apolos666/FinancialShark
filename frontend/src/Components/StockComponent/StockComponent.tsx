import React, {useEffect, useState} from "react";
import {CommentFormInputs, StockComponentForm} from "./StockComponentForm/StockComponentForm";
import {commentGetAPI, commentPostAPI} from "../../Services/CommentService";
import {toast} from "react-toastify";
import {CommentGet} from "../../Models/Comment";
import Spinner from "../Spinner/Spinner";
import {StockCommentList} from "../StockCommentList/StockCommentList";

interface Props {
    stockSymbol: string;
};

export const StockComponent = ({stockSymbol}: Props) => {
    const [comments, setComments] = useState<CommentGet[] | null>(null);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        getComments();
    }, []);

    const handleComment = (
        e: CommentFormInputs) => {
        commentPostAPI(e.title, e.content, stockSymbol)
            .then((res) => {
                if (res) {
                    toast.success("Comment created successfully");
                    getComments();
                }
            }).catch((e) => {
                toast.warning(e);
        })
    }

    const getComments = () => {
        setLoading(true);
        commentGetAPI(stockSymbol)
            .then((res) => {
                if (res) {
                    setLoading(false);
                    setComments(res?.data!);
                }
            })
    }

    return (
        <div className="flex flex-col">
            {loading ? <Spinner /> : <StockCommentList comments={comments!} />}
            <StockComponentForm stockSymbol={stockSymbol} handleComment={handleComment}/>
        </div>
    );
}
