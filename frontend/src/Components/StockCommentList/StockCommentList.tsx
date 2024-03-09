import React from "react";
import {StockCommentListItem} from "../StockCommentListItem/StockCommentListItem";
import {CommentGet} from "../../Models/Comment";

interface Props {
    comments: CommentGet[];
};

export const StockCommentList = ({comments}: Props) => {
    return (
        <div>
            {comments ? comments.map((comment) => {
               return <StockCommentListItem comment={comment} />
            }) : ""}
        </div>
    );
};