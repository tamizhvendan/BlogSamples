type IncomeSource =
| Salary
| Royalty

type ExpenseCategory =
| Food
| Entertainment

type Money = Money of decimal

type Income = {
  Amount : Money
  Source : IncomeSource
} 

type Expense = {
  Amount : Money
  Category : ExpenseCategory
}

type Transaction =
| Credit of Income 
| Debit of Expense



// Transaction list -> Money
let balance transactions =
  transactions
  |> List.map (
    function
    | Credit x -> 
      let (Money m) = x.Amount 
      m
    | Debit y ->
      let (Money m) = y.Amount
      -m
  )
  |> List.sum
  |> Money


let rec private getExpenses' expenseCategory transactions expenses =
  match transactions with
  | [] -> expenses
  | x :: xs -> 
    match x with
    | Debit expense when expense.Category = expenseCategory ->
      (expense :: expenses)
      |> getExpenses' expenseCategory xs
    | _ -> getExpenses' expenseCategory xs expenses

let getExpenses expenseCategory transactions =
  getExpenses' expenseCategory transactions []

let getExpenditure expenseCategory transactions =
  getExpenses expenseCategory transactions
  |> List.map (fun expense -> 
    let (Money m) = expense.Amount 
    m
  )
  |> List.sum
  |> Money


// ExpenseCategory -> Transaction list list -> Money
let averageExpenditure expenseCategory transactionsList =
  transactionsList
  |> List.map (getExpenditure expenseCategory)
  |> List.map (fun (Money m) -> m)
  |> List.average
  |> Money


let transactions = 
  
  [ {Category = Food; Amount = Money 10m}
    {Category = Food; Amount = Money 15m}
  ] |> List.map Debit


let transactions2 = 
  
  [ {Category = Food; Amount = Money 5m}
    {Category = Food; Amount = Money 10m}
  ] |> List.map Debit


averageExpenditure Food [transactions;transactions2]