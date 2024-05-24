import { useFetchTodoItems, usePutTodoItem } from '../../hooks'
import { TodoItem } from '@/models/TodoItem'
import { useEffect, useState } from 'react'
import { Alert, Button, Table } from 'react-bootstrap'

export const TodoItems = () => {
  const { data: items, loading, error, fetchData } = useFetchTodoItems<TodoItem[]>('todoitems')
  const { data: putResult, error: putError, putData } = usePutTodoItem<TodoItem[]>('todoitems')
  const [todoItems, setTodoItems] = useState<TodoItem[] | null>([])

  useEffect(() => {
    if (todoItems == null) {
      return
    }
    for (let i = 0; i < todoItems.length; i++) {
      if (todoItems[i].id == putResult?.toString()) {
        todoItems[i].isCompleted = true
        setTodoItems(todoItems)
        break
      }
    }
  }, [putResult])

  useEffect(() => {
    setTodoItems(items)
  }, [items])

  async function getItems() {
    fetchData()
  }
  async function handleMarkAsComplete(_item: TodoItem) {
    _item.isCompleted = true
    putData(_item, _item.id)
  }
  return (
    <>
      {loading && <div>Loading...</div>}
      {error && (
        <div>
          <Alert key={error} variant={'danger'} dismissible>
            {error}
          </Alert>
        </div>
      )}
      <h1>
        Showing {todoItems?.length} Item(s){' '}
        <Button variant="primary" className="pull-right" onClick={() => getItems()}>
          Refresh
        </Button>
      </h1>
      {error && (
        <div>
          <Alert key={putError} variant={'danger'} dismissible>
            {putError}
          </Alert>
        </div>
      )}
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Id</th>
            <th>Description</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {todoItems?.map((item: TodoItem) => (
            <tr key={item.id} data-testid='todoItem'>
              <td>{item.id}</td>
              <td>{item.description}</td>
              <td>
                {item.isCompleted ? (
                  <div>completed</div>
                ) : (
                  <Button variant="warning" size="sm" onClick={() => handleMarkAsComplete(item)}>
                    Mark as completed
                  </Button>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </>
  )
}
