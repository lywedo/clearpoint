import usePostTodoItem from '../../hooks/usePostTodoItem'
import { TodoItem } from '@/models/TodoItem'
import { generateGUID } from '../../utils/GuidUtil'
import { ChangeEvent, useState } from 'react'
import { Button, Container, Row, Col, Form, Stack, Alert } from 'react-bootstrap'

export const AddTodoItem = () => {
  const [description, setDescription] = useState('')
  const { data, loading, error, postData, cleanData } = usePostTodoItem<TodoItem>('todoitems')
  const handleDescriptionChange = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setDescription(event.target.value)
  }
  async function handleAdd() {
    const newTodo = {
      id: generateGUID(),
      isCompleted: false,
      description: description,
    }
    cleanData();
    postData(newTodo)
  }

  function handleClear() {
    cleanData();
    setDescription('')
  }

  return (
    <Container>
      {loading && <div>Loading...</div>}
      {error && (
        <div>
          <Alert key={error} variant={'danger'} dismissible>
            {error}
          </Alert>
        </div>
      )}
      {data && (
        <div>
          <Alert key={data.id} variant={'success'} dismissible>
            {data.id}
          </Alert>
        </div>
      )}
      {/* <h1>Add Item</h1> */}
      <Form.Group as={Row} className="mb-3" controlId="formAddTodoItem">
        <Form.Label column sm="2">
          Description
        </Form.Label>
        <Col md="6">
          <Form.Control
            type="text"
            placeholder="Enter description..."
            value={description}
            onChange={handleDescriptionChange}
          />
        </Col>
      </Form.Group>
      <Form.Group as={Row} className="mb-3 offset-md-2" controlId="formAddTodoItem">
        <Stack direction="horizontal" gap={2}>
          <Button variant="primary" onClick={() => handleAdd()}>
            Add Item
          </Button>
          <Button variant="secondary" onClick={() => handleClear()}>
            Clear
          </Button>
        </Stack>
      </Form.Group>
    </Container>
  )
}
