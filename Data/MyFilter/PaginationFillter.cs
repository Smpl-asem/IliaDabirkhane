
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        //totalpage
       // public int TotalPages { get; set; }



        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
         
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize < 1  ? 10 : pageSize;
            
        }
        //
    }

    public class PagedResponse<T>
    {
        public PagedResponse(T data, int pageNumber, int pageSize,int totalPages,int totalCount)
        {
            this.Data = data;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalPages=totalPages;
            this.TotalCount=totalCount;
        }
        public T Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        //total page
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }






    

    